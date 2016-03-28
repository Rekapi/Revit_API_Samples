using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Windows;
//using DialogResult = System.Windows.Forms.DialogResult;

namespace XYZTransForm
{
    [Transaction(TransactionMode.Automatic)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class Class1 : IExternalCommand
    {
        #region IExternalCommand Members

        public Autodesk.Revit.UI.Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
        {
            Autodesk.Revit.ApplicationServices.Application app = new Autodesk.Revit.ApplicationServices.Application();
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Selection Selection = uiDoc.Selection;
            SelElementSet ElementSet = Selection.Elements;

            #region UserForm Creation
            System.Windows.Forms.Form Frm = new System.Windows.Forms.Form();
            Frm.Text = "Moving Selected Element";
            Frm.HelpButton = Frm.MinimizeBox = Frm.MaximizeBox = false;
            Frm.ShowIcon = Frm.ShowInTaskbar = false;
            Frm.TopMost = true;
            Frm.Height = 200;
            Frm.Width = 280;
            Frm.MinimumSize = new System.Drawing.Size(Frm.Width, Frm.Height);
            #endregion UserForm Creation

            #region Label Distance Creation
            int margin = 10;
            System.Windows.Forms.Label Lab = new System.Windows.Forms.Label();
            Lab.AutoSize = true;
            Lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Lab.Location = new System.Drawing.Point(margin, margin);
            //Lab.Location = new System.Drawing.Point(25, 24);
            Lab.Name = "Label1";
            Lab.Size = new System.Drawing.Size(60, 15);
            Lab.TabIndex = 8;
            Lab.Text = "&Distance :";
            Frm.Controls.Add(Lab);
            #endregion Label Distance Creation

            #region X-Direction Checkbox
            System.Windows.Forms.CheckBox Xdis = new System.Windows.Forms.CheckBox();
            Xdis.AutoSize = true;
            Xdis.Checked = true;
            Xdis.Location = new System.Drawing.Point(20, 50);
            Xdis.Size = new System.Drawing.Size(80, 30);
            Xdis.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Xdis.TabIndex = 10;
            Xdis.TabStop = true;
            Xdis.Text = "&X-Direction";
            Xdis.UseVisualStyleBackColor = true;
            Frm.Controls.Add(Xdis);
            #endregion X-Direction Checkbox

            #region Y-Direction Checkbox
            System.Windows.Forms.CheckBox Ydis = new System.Windows.Forms.CheckBox();
            Ydis.AutoSize = true;
            Ydis.Checked = true;
            Ydis.Location = new System.Drawing.Point(20, 75);
            Ydis.Size = new System.Drawing.Size(80, 30);
            Ydis.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Ydis.TabIndex = 10;
            Ydis.TabStop = true;
            Ydis.Text = "&Y-Direction";
            Ydis.UseVisualStyleBackColor = true;
            Frm.Controls.Add(Ydis);
            #endregion Y-Direction Checkbox

            # region Text Box creation
            // margin = 10 
            System.Drawing.Size size = Frm.ClientSize;
            System.Windows.Forms.TextBox Tb = new System.Windows.Forms.TextBox();
            Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            Tb.Height = 20;
            Tb.Width = size.Width - 2 * margin - 80;
            Tb.Location = new System.Drawing.Point(margin + 70, margin);
            Tb.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Frm.Controls.Add(Tb);
            #endregion Text Box creation

            # region Move Button Creation
            System.Windows.Forms.Button ok = new System.Windows.Forms.Button();
            ok.Text = "Move";
            ok.Click += new EventHandler(ok_Click);
            ok.Height = 23;
            ok.Width = 75;
            ok.Location = new System.Drawing.Point(Frm.ClientSize.Width / 2 - ok.Width / 2, Frm.ClientSize.Height - 40);
            ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Frm.Controls.Add(ok);
            Frm.AcceptButton = ok;
            #endregion Move Button Creation

            if (!ElementSet.IsEmpty)
            {
                Frm.ShowDialog();
                string dist = Tb.Text;
                double distance = Convert.ToDouble(dist);
                double ConvertedValue = distance / 0.3048;
                if (Tb.Text != "")
                {
                    if (Xdis.Checked==true && Ydis.Checked==true)
                    {

                        XYZ xyz = new XYZ(ConvertedValue, ConvertedValue, 0);
                        doc.Move(ElementSet, xyz);
                        
                    }
                    else if (Xdis.Checked == true && Ydis.Checked == false)
                    {
                        XYZ xyz = new XYZ(ConvertedValue, 0, 0);
                        doc.Move(ElementSet, xyz);
                    }
                    else
                    {
                        XYZ xyz = new XYZ(0, ConvertedValue, 0);
                        doc.Move(ElementSet, xyz);
                    }
                }
                else
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Error", "Please full the distance field");
                    Frm.Show();
                }
                
            }
            else
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", "Sorry, you must select an Element first ..");
                Frm.Close();
            }
            return Result.Succeeded;
        }
        static void ok_Click( object sender, EventArgs e )
        {
            System.Windows.Forms.Form form = (sender as System.Windows.Forms.Control).Parent as System.Windows.Forms.Form;
            form.DialogResult = System.Windows.Forms.DialogResult.OK;
            form.Close();
        }
        #endregion
    }
}
