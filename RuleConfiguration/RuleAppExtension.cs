using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sas = System.Activities.Statements;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using System.Runtime.InteropServices;
using Autodesk.IM.Rule;
using Autodesk.IM.UI.Rule;
using RuleConfiguration;
using Autodesk.IM.Rule.Activities;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;


[assembly: Autodesk.AutoCAD.Runtime.ExtensionApplication(typeof(RuleConfiguration.RuleAppExtension))]
[assembly: CommandClass(typeof(RuleConfiguration.RuleAppExtension))]
namespace RuleConfiguration
{
    public class RuleAppExtension : IExtensionApplication
    {
        [STAThread]
        public static void Main()
        {
            RuleAppExtension app = new RuleAppExtension();
            
            app.ConfigureRule();

            //TestValidationPanel();
        }

        private static void TestValidationPanel()
        {
            System.Windows.Application app = new System.Windows.Application();
            ValidationPanelDialog dialog = new ValidationPanelDialog();
            DesignValidationContext context = new DesignValidationContext();
            RuleAppExtension.ValidationManager.AddValidationItem(new PipeValidationItem
            {
                FID = 1,
                DeviceType = "Pipe",
                Category = "General Validation",
                ResultType = ValidationType.Error,
                Title = "Validation Failed",
            });

            RuleAppExtension.ValidationManager.AddValidationItem(new PipeValidationItem
            {
                FID = 2,
                DeviceType = "Pipe",
                Category = "General Validation",
                ResultType = ValidationType.Error,
                Title = "Validation Failed",
            });

            RuleAppExtension.ValidationManager.AddValidationItem(new PipeValidationItem
            {
                FID = 3,
                DeviceType = "Pipe",
                Category = "General Validation",
                ResultType = ValidationType.Error,
                Title = "Validation Failed"
            });

            RuleAppExtension.ValidationManager.AddValidationItem(new PipeValidationItem
            {
                FID = 4,
                DeviceType = "Pipe",
                Category = "General Validation",
                ResultType = ValidationType.Error,
                Title = "Validation Failed",
            });
            dialog.DataContext = context;
            app.Run(dialog);
        }

        [CommandMethod("ConfigureRule")]
        public void ConfigureRule()
        {
            InitializeRule();

            ((RuleLibrary)RuleManagerInstance.Storage).Load();

            // Create a default rule
            RulePoint rp = RuleManagerInstance.GetRulePoint("/PipeRule");
            if (!rp.HasActivity)
            {
                rp.CreateDefaultWorkflow();
            }            

            System.Windows.Application app = new System.Windows.Application();
            RuleConfigDialog dialog = RuleConfigDialog.GetInstance();
            dialog.SelectedRulePath = "/PipeRule";
            app.Run(dialog);

            ((RuleLibrary)RuleManagerInstance.Storage).Save();
        }

        [CommandMethod("ExecuteRule")]
        public static void ExecuteRule()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            ValidationRuleResult result = new ValidationRuleResult();

                args.Add(PipeRuleSignature.PipeArgument.Name, new object());
                args.Add(PipeRuleSignature.ResultArgument.Name, result);
                RuleManagerInstance.InvokeRulePoint("/PipeRule", args);
            
        }

        //[CommandMethod("PrintAllPressurePipes")]
        //public void PrintAllPressurePipes()
        //{
        //    ObjectIdCollection pipeIds = PipeIdsCollection();
        //}

        public static ObjectIdCollection PipeIdsCollection()
        {
            ObjectIdCollection pipeIds = new ObjectIdCollection();
            ObjectIdCollection networkIds = CivilDocumentPressurePipesExtension.GetPressurePipeNetworkIds(CivilApplication.ActiveDocument);
            Autodesk.AutoCAD.EditorInput.Editor m_deditor = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction trans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
            {
                foreach (ObjectId id in networkIds)
                {
                    PressurePipeNetwork networkObj = trans.GetObject(id, OpenMode.ForRead) as PressurePipeNetwork;
                    pipeIds = networkObj.GetPipeIds();
                    foreach (ObjectId pipeId in pipeIds)
                    {
                        // Test code
                        PressurePipe pipeObj = trans.GetObject(pipeId, OpenMode.ForRead) as PressurePipe;


                        m_deditor.WriteMessage("Pipe Name: {0}\n", pipeObj.Name);
                        m_deditor.WriteMessage("Pipe Material: {0}\n", pipeObj.PipeMaterial);
                        m_deditor.WriteMessage("Pipe InnerDiameter: {0}\n", pipeObj.InnerDiameter);
                        m_deditor.WriteMessage("Pipe Network Name: {0}\n", pipeObj.NetworkName);
                        // End test

                        //args.Add(PipeRuleSignature.PipeArgument.Name, pipeObj);
                        //args.Add(PipeRuleSignature.ResultArgument.Name, result);
                        //RuleManagerInstance.InvokeRulePoint("/PipeRule", args);
                    }
                }
            }

            return pipeIds;
        }

        public static void InitializeRule()
        {
            // Register rule point
            PipeRuleSignature signature = new PipeRuleSignature(RuleManagerInstance);
            RuleManagerInstance.RegisterRulePoint("/", "PipeRule", "Pipe Rule Demo", signature);

            // Register activities
            RegisterActivities();

            RegisterOperators();

            // Initialize UI
            RegisterDesigners();
            InitializeActivityTranslator();
        }


        private static void RegisterOperators()
        {
            RuleActivityManager ruleActivityManager = RuleManagerInstance.ActivityManager;
            DynamicOperators.RegisterOperators(ruleActivityManager);
        }


        private static void RegisterActivities()
        {
            RuleActivityManager ruleActivityManager = RuleManagerInstance.ActivityManager;            
            ActivitySignature generalSignature = new ActivitySignature();
            PipeRuleSignature pipeRuleSignature = new PipeRuleSignature(RuleManagerInstance);
            ruleActivityManager.RegisterActivity(
                "If",   // NOXLATE
                "If",
                () => new If(),
                generalSignature,
                typeof(If));
            ruleActivityManager.RegisterActivity(
                "Report",
                "Report",
                AddValidationItem.Create,
                pipeRuleSignature,
                typeof(AddValidationItem));
        }


        private static void InitializeActivityTranslator()
        {
            AudActivityTranslateItems.AddActivityTranslateItems(RuleAppExtension.ActivityTranslatorInst);
        }


        private static void RegisterDesigners()
        {
            RuleDesignerManager ruleDesignerManager = RuleDesignerManagerInst;

            // Register designer for activities
            ruleDesignerManager.RegisterDesigner(typeof(If), typeof(CompositeIfDesigner));
            ruleDesignerManager.RegisterDesigner(typeof(sas.If), typeof(CompositeIfDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(Match), typeof(MatchDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(MatchWithResult), typeof(MatchWithResultDesigner));
            ruleDesignerManager.RegisterDesigner(typeof(sas.Sequence), typeof(SequenceDesigner));
            ruleDesignerManager.RegisterDesigner(typeof(StringExpression), typeof(StringExpressionDesigner));
            ruleDesignerManager.RegisterDesigner(typeof(PipePropertyActivity), typeof(PipePropertyDesigner));
            ruleDesignerManager.RegisterDesigner(typeof(AddValidationItem), typeof(ValidationActivityDesigner));            

            //ruleDesignerManager.RegisterDesigner(typeof(AddValidationItem), typeof(ValidationActivityDesigner));

            //ruleDesignerManager.RegisterDesigner(typeof(AddContainedFeature), typeof(AddFeatureActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(UpdateProperty), typeof(UpdatePropertyDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(UpdateMultipleProperties), typeof(UpdateMultiplePropertiesDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(BuildResolution), typeof(ResolutionBuildActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(PromptResolve), typeof(PromptResolveActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(PromptSetModelName), typeof(SetModelResolveActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(PromptSetProperty), typeof(SetAttributeResolveActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(EnsurePropertySet), typeof(EnsureAttributeSetDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(InvokeNamedRule), typeof(InvokeNamedRuleDesigner));

            //ruleDesignerManager.RegisterDesigner(typeof(AddMaterial), typeof(MaterialAddActivityDesigner));

            // Register designer for operators
            RuleManager ruleManager = RuleManagerInstance;
            RuleActivityManager activityManager = ruleManager.ActivityManager;
            foreach (var item in activityManager.GetOperatorRegistries())
            {
                if (item.Category == OperatorEntry.OperatorCategory.Binary)
                {
                    ruleDesignerManager.RegisterDesigner(item.ActivityType, typeof(HorizontalBinaryDesigner));
                }
                else
                {
                    ruleDesignerManager.RegisterDesigner(item.ActivityType, typeof(UnaryOperatorDesigner));
                }
            }

            // Register designer for functions
            //ruleDesignerManager.RegisterDesigner(typeof(FeaturePropertyIsSet), typeof(FeaturePropertyExistDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(FeaturePropertyNotSet), typeof(FeaturePropertyNotExistDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ListToTextActivity), typeof(ListToTextDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(TableLookup), typeof(TableLookupDesigner));

            //ruleDesignerManager.RegisterDesigner(typeof(ConnectedCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ConnectedAtStartCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ConnectedAtEndCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ContainedCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ContainedSiblingCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ContainedConnectedCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ContainerExist), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(DownstreamCount), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(UpstreamExists), typeof(ConnectTracingFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ConditionalValue<DynamicValue>), typeof(ConditionalValueDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ConditionalValue<string>), typeof(ConditionalValueDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(FeatureClassName), typeof(FunctionNoParameterDesigner));

            //ruleDesignerManager.RegisterDesigner(typeof(MathFunctionWithOneArg), typeof(FunctionOneParameterDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(MathFunctionWithTwoArgs), typeof(FunctionTwoParametersDesigner));

            // Register designer for expression items
            //ruleDesignerManager.RegisterDesigner(typeof(FeatureItemProperty), typeof(PipePropertyDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(Expression<>), typeof(ExpressionActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(ExpressionWithUnit), typeof(ExpressionWithUnitActivityDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(CatalogMaterialProperty), typeof(CatalogMaterialAttributeDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(aura.AudVariable), typeof(AudVariableDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(DomainValue), typeof(FunctionNoParameterDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(NamedRuleFunction), typeof(NamedRuleFunctionDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(DocumentProperty), typeof(DocumentPropertyDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(CatalogItemValue), typeof(CatalogItemValueDesigner));

            // Register designer for sizing rules
            //ruleDesignerManager.RegisterDesigner(typeof(AddQueryMatch), typeof(AddQueryMatchDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(TableQueryMatch), typeof(TableQueryMatchDesigner));
            //ruleDesignerManager.RegisterDesigner(typeof(AddSortAttribute), typeof(AddSortAttributeDesigner));

            //ruleDesignerManager.RegisterDesigner(typeof(ChooseStyle), typeof(ChooseStyleActivityDesigner));
        }



        private static RuleManager _ruleManagerInstance = null;
        public static RuleManager RuleManagerInstance
        {
            get
            {
                if (_ruleManagerInstance == null)
                {
                    _ruleManagerInstance = new RuleManager();
                    Initialize(_ruleManagerInstance);
                }
                return _ruleManagerInstance;
            }
        }


        private static RuleDesignerManager _ruleDesignerManagerInst = null;
        public static RuleDesignerManager RuleDesignerManagerInst
        {
            get
            {
                if (_ruleDesignerManagerInst == null)
                {
                    _ruleDesignerManagerInst = new RuleDesignerManager();
                }
                return _ruleDesignerManagerInst;
            }
        }

        private static ActivityTranslator _activityTranslator = null;
        public static ActivityTranslator ActivityTranslatorInst
        {
            get
            {
                if (_activityTranslator == null)
                {
                    _activityTranslator = new ActivityTranslator();
                }
                return _activityTranslator;
            }
        }


        private static ActivityFactory _activityFactoryInst = null;
        public static ActivityFactory ActivityFactoryInst
        {
            get
            {
                if (_activityFactoryInst == null)
                {
                    _activityFactoryInst = new ActivityFactory();
                }
                return _activityFactoryInst;
            }
        }


        private static ValidationManager _validationManager = null;
        public static ValidationManager ValidationManager
        {
            get
            {
                if (_validationManager == null)
                {
                    _validationManager = new ValidationManager();
                }
                return _validationManager;
            }
        }


        private static void Initialize(RuleManager _ruleManagerInstance)
        {
            _ruleManagerInstance.Storage = new RuleConfiguration.RuleLibrary();
        }


        //private Autodesk.AutoCAD.EditorInput.Editor m_editor = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        public void Initialize()
        {
            //m_editor.WriteMessage("Initialize()\n");
        }

        public void Terminate()
        {
            //m_editor.WriteMessage("Terminate()\n");
        }

        [CommandMethod("TestRuleOptions")]
        public void TestRuleOptions()
        {
            //m_editor.WriteMessage("Run Test Rule Options command\n");
            System.Windows.MessageBox.Show("step3");
            RuleConfigDialog dialog = RuleConfigDialog.GetInstance();
            System.Windows.MessageBox.Show("step4");
            dialog.Show();
        }
    }
}
