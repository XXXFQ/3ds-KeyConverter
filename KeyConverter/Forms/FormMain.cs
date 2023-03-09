using System;
using System.Windows.Forms;

using KeyConverter.Properties;
using KeyConverter.Utils;

namespace KeyConverter.Forms
{
    public partial class FormMain : Form
    {
        // キーボックスの数
        const int KEY_CHEAK_BOX_LENGTH = 23;

        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームがロードされた時
        /// </summary>
        private void FormMain_Load(object sender, EventArgs e)
        {
            Txt_KeyCodeBox.Tag = 0;

            for (int bit = 0; bit < KEY_CHEAK_BOX_LENGTH; bit++)
            {
                CheckBox keyCheckBox = (CheckBox)TabPage1.Controls[$"KeyCheckBox{bit + 1}"];
                keyCheckBox.CheckedChanged += KeyCheckBoxs_Cheaked;

                if (bit <= 11) {
                    keyCheckBox.Tag = 1 << bit;
                }
                // keyが"ZL"か"ZR"だった場合
                else if (12 <= bit && bit <= 13) {
                    keyCheckBox.Tag = 1 << (bit + 2);
                }
                // keyが"Touch Screen"だった場合
                else if (bit == 14) {
                    keyCheckBox.Tag = 1 << (bit + 6);
                }
                else {
                    keyCheckBox.Tag = 1 << (bit + 9);
                }
            }
        }

        /// <summary>
        /// ツールのバージョン情報を表示
        /// </summary>
        private void Tsmi_Version_Click(object sender, EventArgs e)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = fileVersionInfo.FileDescription;

            string message = $"{title} v{assemblyName.Version.ToString(3)}\n{fileVersionInfo.LegalCopyright}";
            MessageBox.Show(message, Resources.VersionInfo, MessageBoxButtons.OK);
        }

        /// <summary>
        /// ツールの終了処理
        /// </summary>
        private void Tsmi_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// キーラベルと連動してキーコード生成
        /// </summary>
        private void KeyCheckBoxs_Cheaked(object sender, EventArgs e)
        {
            CheckBox Chk_Key = (CheckBox)sender;
            int keyValue = Convert.ToInt32(Chk_Key.Tag);
            int keyCode = Convert.ToInt32(Txt_KeyCodeBox.Tag);

            // キーコードの計算
            keyCode += Chk_Key.Checked ? keyValue : -keyValue;

            // キーボックスを更新
            Txt_KeyCodeBox.Text = $"DD000000 {keyCode:X8}";
            Txt_KeyCodeBox.Tag = keyCode;
        }

        /// <summary>
        /// キーコードをクリップボードにコピー
        /// </summary>
        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Txt_KeyCodeBox.Text);
        }

        /// <summary>
        /// キーをリセットする
        /// </summary>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= KEY_CHEAK_BOX_LENGTH; i++) {
                CheckBox keyCheckBox = (CheckBox)TabPage1.Controls[$"KeyCheckBox{i}"];
                keyCheckBox.Checked = false;
            }
            Txt_KeyCodeBox.Text = "DD000000 00000000";
            Txt_KeyCodeBox.Tag = 0;
        }

        /// <summary>
        /// テキストボックスが空の場合、Convertボタンを無効にする(逆変換)
        /// </summary>
        private void KeyText_Re_TextChanged(object sender, EventArgs e)
        {
            Btn_Convert_Re.Enabled = Txt_Key_Re.Text != "";
        }

        /// <summary>
        /// キーコードを逆変換する
        /// </summary>
        private void ConvertButton_Re_Click(object sender, EventArgs e)
        {
            // 正しいキーの値か確認
            if (!StringExtensions.IsHexString(Txt_Key_Re.Text))
            {
                MessageBox.Show(Resources.EnterHexNumber, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int keyValue = Convert.ToInt32(Txt_Key_Re.Text, 16);
            string keyText = "";

            for (int bit = 0; bit < KEY_CHEAK_BOX_LENGTH; bit++)
            {
                // 指定されたキーを確認
                if (Convert.ToBoolean(keyValue & 1)) {
                    CheckBox keyCheckBox = (CheckBox)TabPage1.Controls[$"KeyCheckBox{bit + 1}"];
                    keyText += $"({keyCheckBox.Text}) + ";
                }

                switch (bit) {
                    case 11: // keyが"Y"だった場合
                        keyValue >>= 3;
                        break;
                    case 13: // keyが"ZR"だった場合
                        keyValue >>= 5;
                        break;
                    case 14: // keyが"Touch Screen"だった場合
                        keyValue >>= 4;
                        break;
                    default:
                        keyValue >>= 1;
                        break;
                }

                if (!Convert.ToBoolean(keyValue)) break;
            }

            // 結果を出力
            if (keyText != "") {
                Txt_OutputKey_Re.Text = keyText.Remove(keyText.Length - 3);
            }
        }

        /// <summary>
        /// キーをリセットする(逆変換)
        /// </summary>
        private void ResetButton_Re_Click(object sender, EventArgs e)
        {
            Txt_Key_Re.Text = "00000000";
            Txt_OutputKey_Re.Text = "";
        }
    }
}