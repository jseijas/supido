using Supido.Structure;
using Supido.Structure.Meta;
using Supido.Templates;
using Supido.Templates.Engine;
using Supido.Templates.Parsing.Evaluators;
using System;
using System.Windows.Forms;

namespace TestBed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StructureParser parser = new StructureParser(this.richTextBox1.Text);
            StructureTree root = parser.Parse();
            MetaModel metamodel = root.Build();
            MetaTemplateConverter converter = new MetaTemplateConverter();
            TemplateContainer container = converter.Convert(metamodel);
            string template = "Domains:\n========\n[%foreach(domain in Domains)%]\n	Id: [%domain.Id%]\n	Name: [%domain.Name%]\n	IsIdentity: [%domain.IsIdentity%]\n	DataType: [%domain.DataType%]\n	Length: [%domain.DataTypeLength%]\n	Decimals: [%domain.DataTypeDecimals%]\n[%end%]\n";
            template += "Tables:\n=======\n[%foreach(table in Tables)%]\n	Id: [%table.Id%]\n	Name: [%table.Name%]\n	LogicalName: [%table.LogicalName%]\n	PhysicalName: [%table.PhysicalName%]\n	[\n[%foreach(column in table.Columns)%]		TableId: [%column.TableId%]\n		Ix: [%column.Ix%]\n		Name: [%column.Name%]\n		LogicalName: [%column.LogicalName%]\n		PhysicalName: [%column.PhysicalName%]\n		IsPrimaryKey: [%column.IsPrimaryKey%]\n		IsNullable: [%column.IsNullable%]\n		IsIdentity: [%column.IsIdentity%]\n		DataType: [%column.DataType%]\n		Length: [%column.DataTypeLength%]\n		Decimals: [%column.DataTypeDecimals%]\n		IsForeignKey: [%column.IsForeignKey%]\n	[%end%]\n][%end%]\n";
            template = TemplateTransformer.Transform(template);
            template = JavascriptEvaluator.ReplaceString(template, container);
            this.richTextBox2.Text = template;
        }

        private string Evaluate(string s)
        {
            return s+" = "+ArithmeticEvaluator.Eval(s).ToString()+"\n";
        }

        public static string Test(string s, string expected, TemplateContainer container)
        {
            string result = JavascriptEvaluator.RunScript(s, container).ToString();
            if (result.Equals(expected))
            {
                return string.Format("OK '{0}' = {1}\n", s, result);
            }
            else
            {
                return string.Format("Error executing '{0}'. Expected {1} but obtained {2}\n", s, expected, result);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            TemplateContainer container = new TemplateContainer();
            container.AddAttribute("varprueba", "prueba");
            container.AddAttribute("vartrans", "transformación");
            container.AddAttribute("i", 7);
            string s = "El resultado es [%if(i > 7)%]cierto[%else%]falso";
            s = TemplateTransformer.Transform(s);
            s = JavascriptEvaluator.ReplaceString(s, container);
            this.richTextBox2.Text = s;
        }
    }
}
