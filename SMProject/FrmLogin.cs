using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Models;
using DAL;

namespace SMProject
{
    public partial class FrmLogin : Form
    {
        private SalesPersonService objSalesPersonService = new SalesPersonService();
        private LoginLogService objLogService = new LoginLogService();
        public FrmLogin()
        {
            InitializeComponent();
        }
        //登录按钮
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.txtLoginId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入账号", "提示信息");
                this.txtLoginId.Focus();
                return;
            }
            if (this.txtLoignPwd.Text.Length == 0)
            {
                MessageBox.Show("请输入密码", "提示信息");
                this.txtLoignPwd.Focus();
                return;
            }
            if (!DataValidate.IsInteger(this.txtLoginId.Text.Trim()))
            {
                MessageBox.Show("请输入正确的账号", "提示信息");
                this.txtLoginId.Focus();
                return;
            }
            SalesPerson salesPerson = new SalesPerson()
            {
                SalesPersonId = Convert.ToInt32(this.txtLoginId.Text.Trim()),
                LoginPwd = this.txtLoignPwd.Text
            };
            try
            {
                salesPerson = objSalesPersonService.SalesPersonLogin(salesPerson);
                if (salesPerson != null)
                {
                    LoginLog currentLog = new LoginLog()
                    {
                        LoginId = salesPerson.SalesPersonId,
                        SPName = salesPerson.SPName,
                        ServerName = Dns.GetHostName()
                    };
                    salesPerson.LogId = objLogService.WriteLoingLog(currentLog);
                    Program.currentSalesPerson = salesPerson;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("账号或密码错误", "登录失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("系统出现异常：" + ex.Message); ;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtLoginId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoginId.Text.Trim().Length != 0 && e.KeyValue == 13)
            {
                this.txtLoignPwd.SelectAll();
                this.txtLoignPwd.Focus();
            }
        }

        private void TxtLoignPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtLoignPwd.Text.Length != 0 && e.KeyValue == 13)
            {
                btnLogin_Click(null, null);
            }
        }
    }
}
