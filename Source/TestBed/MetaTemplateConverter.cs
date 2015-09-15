using Supido.Structure.Meta;
using Supido.Templates;

namespace TestBed
{
    public class MetaTemplateConverter
    {
        private MetaModel model;

        public MetaTemplateConverter()
        {
        }

        private void BuildDomains(TemplateContainer node)
        {
            foreach (MetaDomain domain in model.Domains)
            {
                TemplateContainer arrnode = node.AddArrayValue(domain.Id.ToString());
                arrnode.AddFromObject(domain);
            }
        }

        private void BuildTables(TemplateContainer node)
        {
            foreach (MetaTable table in model.Tables)
            {
                TemplateContainer arrnode = node.AddArrayValue(table.Id.ToString());
                arrnode.AddFromObject(table);
                TemplateLink listLink = arrnode.AddListLink("Columns", "/Columns");
                foreach (MetaColumn column in table.Columns)
                {
                    listLink.AddListValue(table.Id.ToString() + "!" + column.Ix.ToString());
                }
            }
        }

        public void BuildColumns(TemplateContainer node)
        {
            foreach (MetaTable table in model.Tables)
            {
                foreach (MetaColumn column in table.Columns)
                {
                    TemplateContainer arrnode = node.AddArrayValue(table.Id.ToString() + "!" + column.Ix.ToString());
                    arrnode.AddFromObject(column);
                    arrnode.AddLink("Table", "Tables[" + column.TableId.ToString() + "]");
                    MetaColumn currentcolumn = column;
                    while (currentcolumn.TargetColumn != null)
                    {
                        currentcolumn = currentcolumn.TargetColumn;
                    }
                    node.AddLink("Domain", "/Domains[" + currentcolumn.Domain.Id.ToString() + "]");
                }
            }
        }

        public TemplateContainer Convert(MetaModel metamodel)
        {
            this.model = metamodel;
            TemplateContainer root = new TemplateContainer();
            root.AddAttribute("Test", "OK");
            this.BuildDomains(root.AddChild("Domains"));
            this.BuildColumns(root.AddChild("Columns"));
            this.BuildTables(root.AddChild("Tables"));
            return root;
        }
    }
}
