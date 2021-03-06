using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;

namespace SMProject
{
    public partial class FrmBalance : Form
    {
        private MemberService objMemberService = new MemberService();
        //构造方法
        public FrmBalance(string totalMoney)
        {
            InitializeComponent();
            //初始化应收金额
            this.lblTotalMoney.Text = totalMoney;
            this.txtRealReceive.Text = totalMoney;
            this.txtRealReceive.SelectAll();
            this.txtRealReceive.Focus();
            //窗体接收keydown事件
            this.KeyPreview = true;
        }

        private void FrmBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 115)//F4：返回
            {
                this.Tag = "F4";
                this.DialogResult = DialogResult.Cancel;
            }
            else if (e.KeyValue == 116)//F5：取消交易
            {
                this.Tag = "F5";
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void TxtMemberId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (!DataValidate.IsDecimalNum(this.txtRealReceive.Text.Trim()) || Convert.ToDecimal(this.txtRealReceive.Text.Trim()) < Convert.ToDecimal(this.lblTotalMoney.Text))
                {
                    MessageBox.Show("请输入正确的实收款", "提示信息");
                    this.txtRealReceive.SelectAll();
                    this.txtRealReceive.Focus();
                    return;
                }
                //不输入会员卡则直接结算
                if (this.txtMemberId.Text.Trim().Length == 0)
                {
                    this.Tag = this.txtRealReceive.Text.Trim();
                    this.DialogResult = DialogResult.OK;
                    return;
                }
                //2147483647
                if (this.txtMemberId.Text.Trim().Contains("|") || !DataValidate.IsInteger(this.txtMemberId.Text.Trim()) || !int.TryParse(this.txtMemberId.Text.Trim(), out int memberId))
                {
                    MessageBox.Show("请输入正确的卡号", "提示信息");
                    this.txtMemberId.SelectAll();
                    this.txtMemberId.Focus();
                    return;
                }
                try
                {
                    //查询会员卡
                    bool isMemberExists = objMemberService.IsMemberExists(this.txtMemberId.Text.Trim());
                    if (!isMemberExists)
                    {
                        MessageBox.Show("没有找到会员，请检查会员号是否输入正确", "提示信息");
                        this.txtMemberId.SelectAll();
                        this.txtMemberId.Focus();
                        return;
                    }
                    else
                    {
                        this.Tag = this.txtRealReceive.Text.Trim() + "|" + this.txtMemberId.Text.Trim();
                        this.DialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询过程中出现异常，请检查会员号是否正确：" + ex.Message);
                    return;
                }
            }
        }

        private void FrmBalance_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Tag == null)
            {
                this.Tag = "F4";
            }
        }

        private void TxtRealReceive_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtRealReceive.Text.Trim().Length != 0 && e.KeyValue == 13)
            {
                if (!DataValidate.IsDecimalNum(this.txtRealReceive.Text.Trim()) || Convert.ToDecimal(this.txtRealReceive.Text.Trim()) < Convert.ToDecimal(this.lblTotalMoney.Text))
                {
                    MessageBox.Show("请输入正确的实收款", "提示信息");
                    this.txtRealReceive.SelectAll();
                    this.txtRealReceive.Focus();
                    return;
                }
                else
                {
                    this.txtMemberId.SelectAll();
                    this.txtMemberId.Focus();
                }
            }

        }
    }
}
