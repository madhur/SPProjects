using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.SharePoint.WebPartPages;
using MySiteLib;
using System.Web.UI.WebControls.WebParts;
using Ref=System.Reflection;

namespace MySiteWebPartsPropertyChanger
{
    public partial class Main : Form
    {
        #region Private variables

        MySiteLib.MySiteLib _mySiteObj;
        SPLimitedWebPartCollection _wColl;
        DataSet dtWpData,dtOriginal;
        DataTable _changes;

        #endregion

        #region Public Methods

        public Main()
        {
            InitializeComponent();



        }

        #endregion

        #region Events
        private void Main_Load(object sender, EventArgs e)
        {
            _mySiteObj = MySiteLib.MySiteLib.GetInstance();
            if (_mySiteObj == null)
            {
                MessageBox.Show(Constants.serverContextError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                 Application.Exit();
            }


           // dgProperties.DataError += new DataGridViewDataErrorEventHandler(dgProperties_DataError);
           // dgProperties.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgProperties_DataBindingComplete);
            dgProperties.CellValueChanged += new DataGridViewCellEventHandler(dgProperties_CellValueChanged);
           // dgProperties.CellValuePushed += new DataGridViewCellValueEventHandler(dgProperties_CellValuePushed);
            //dgProperties.CellValidating += new DataGridViewCellValidatingEventHandler(dgProperties_CellValidating);
            lstWebParts.SelectedIndexChanged += new EventHandler(lstWebParts_SelectedIndexChanged);
            //lstWebParts.c
            //dgProperties.RowsAdded += new DataGridViewRowsAddedEventHandler(dgProperties_RowsAdded);
            _changes = new DataTable();
            _changes.Columns.Add("WebPart");
            _changes.Columns.Add("Property");
            _changes.Columns.Add("Value");
            _changes.Columns.Add("PropertyType");

            dtWpData = new DataSet();
            if (_mySiteObj != null)
                lblMySiteUrl.Text = _mySiteObj.Initialize();
            else
                return;


            
            LoadControls();

            
        }

        private void dgProperties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dtOriginal.Tables[lstWebParts.SelectedIndex].Rows[e.RowIndex][e.ColumnIndex].ToString() != dgProperties[e.ColumnIndex, e.RowIndex].Value.ToString())
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Font = new Font(dgProperties.Font, FontStyle.Bold);
                dgProperties["Property", e.RowIndex].Style = style;

                dtWpData.Tables[lstWebParts.SelectedIndex].Rows[e.RowIndex]["IsDirty"] = true;

                WebPartItem partItem=lstWebParts.SelectedItem as WebPartItem;
                _changes.Rows.Add(partItem.TypeName, dgProperties["Property", e.RowIndex].Value, dgProperties["Value", e.RowIndex].Value.ToString(), dgProperties["PropertyType", e.RowIndex].Value.ToString());
            }
            else
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Font = new Font(dgProperties.Font, FontStyle.Regular);
                dgProperties["Property", e.RowIndex].Style = style;
                dtWpData.Tables[lstWebParts.SelectedIndex].Rows[e.RowIndex]["IsDirty"] = false;
                DataRow []rows=_changes.Select("Property='" + dgProperties["Property", e.RowIndex].Value+"'");
                
                foreach (DataRow row in rows)
                {
                    _changes.Rows.Remove(row);
                }

            }

           // throw new NotImplementedException();
        }

        private void lstWebParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgProperties.DataSource = dtWpData.Tables[lstWebParts.SelectedIndex];
            dgProperties.Columns["Type"].Visible = false;
            dgProperties.Columns["Assembly"].Visible = false;
            dgProperties.Columns["IsDirty"].Visible = false;
            dgProperties.Columns["Property"].ReadOnly = true;
            dgProperties.Columns["PropertyType"].Visible = false;

            int index = 0;
            foreach (DataRow row in dtWpData.Tables[lstWebParts.SelectedIndex].Rows)
            {
                string dataType = row[1].ToString();
                if (dataType == typeof(Boolean).ToString())
                {
                    dgProperties[2, index] = new DataGridViewCheckBoxCell(false);
                    dgProperties[2, index].Value = row[2].ToString();
                }
                else if (dataType == typeof(String).ToString() || dataType == typeof(Int32).ToString())
                {
                }
                else
                {
                    dgProperties[2, index] = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell comboCell = dgProperties[2, index] as DataGridViewComboBoxCell;
                    comboCell.Items.Clear();
                    Ref.FieldInfo[] fldInfos = Type.GetType(row["Assembly"].ToString()).GetFields();
                      foreach (Ref.FieldInfo fldInfo in fldInfos)
                        {
                            if (!fldInfo.IsSpecialName)
                            {
                                comboCell.Items.Add(fldInfo.Name);
                            }
                        }
                    comboCell.ValueType = typeof(string);
                    comboCell.Value = row["Value"].ToString();


                }
                index++;
            }
        
        }

        private void LoadControls()
        {

            int index = 0;

            dtWpData.Tables.Clear();
            lstWebParts.Items.Clear();
            lstWebParts.ClearSelected();
            dgProperties.DataSource = null;
            dgProperties.ClearSelection();
            _changes.Rows.Clear();

            try
            {
                _wColl = _mySiteObj.GetWebPartsList();
            }
            catch (MySiteDoesNotExistException ex)
            {

                MessageBox.Show(ex.Message, "Error");
                return;
            }
            catch (UserNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

            foreach (System.Web.UI.WebControls.WebParts.WebPart wpPart in _wColl)
            {
                #region Add Webparts to ListBox and and there properties

                lstWebParts.Items.Add(new WebPartItem(wpPart.Title, wpPart.ID, wpPart.GetType().AssemblyQualifiedName));
                dtWpData.Tables.Add(new DataTable());
                dtWpData.Tables[index].Columns.Add("Property");
                dtWpData.Tables[index].Columns.Add("Type");

                dtWpData.Tables[index].Columns.Add("Value");
                dtWpData.Tables[index].Columns.Add("Assembly");
                dtWpData.Tables[index].Columns.Add("IsDirty");
                dtWpData.Tables[index].Columns.Add("PropertyType");

                Ref.PropertyInfo[] propInfoColl = wpPart.GetType().GetProperties();


                foreach (Ref.PropertyInfo propInfo in propInfoColl)
                {
                    Object[] attribColl = propInfo.GetCustomAttributes(true);

                    foreach (Object obj in attribColl)
                    {
                        #region Add Attribute to DT

                        if (obj is WebBrowsableAttribute)
                        {
                            WebBrowsableAttribute browAtt = (WebBrowsableAttribute)obj;

                            if (browAtt.Browsable)
                            {
                                Object value = propInfo.GetValue(wpPart, null);
                                dtWpData.Tables[index].Rows.Add(propInfo.Name, propInfo.PropertyType, value, propInfo.PropertyType.AssemblyQualifiedName,false,propInfo.PropertyType.ToString());


                                break;

                            }
                        }

                        else if (obj is WebPartStorageAttribute)
                        {
                            WebPartStorageAttribute storAtt = obj as WebPartStorageAttribute;
                            if (storAtt.Storage == Storage.Personal)
                            {
                                Object value = propInfo.GetValue(wpPart, null);

                                dtWpData.Tables[index].Rows.Add(propInfo.Name, propInfo.PropertyType, value, propInfo.PropertyType.AssemblyQualifiedName,false,propInfo.PropertyType.ToString());

                                break;


                            }

                        }
                        #endregion

                    }


                }

                index++;

            }

            dtOriginal = dtWpData.Copy();



                #endregion
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                _wColl = _mySiteObj.GetWebPartsList();
            }
            catch (MySiteDoesNotExistException ex)
            {

                MessageBox.Show(ex.Message, "Error");
                return;
            }

            LoadControls();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            DialogResult result=MessageBox.Show(Constants.commitConfirmMessage, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    lblErrors.Text = _mySiteObj.CommitChanges(_changes);
                }
                catch (MySiteDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return;
                }
                MessageBox.Show("Done");

                LoadControls();
            }

            
          
        }

        private void lstWebParts_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
