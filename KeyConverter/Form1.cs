using System;
using System.Windows.Forms;

namespace KeyConverter
{
    public partial class Form1 : Form
    {
        // キーボックスの数
        public const int KEY_CHEAK_BOX_LENGTH = 23;

        // 16進文字列か判定
        public bool IsHexString(string str)
        {
            if (string.IsNullOrEmpty(str)){
                return false;
            }
            foreach (char c in str) {
                if (!Uri.IsHexDigit(c)) {
                    return false;
                }
            }
            return true;
        }

        public Form1()
        {
            InitializeComponent();
            for (int i = 1; i <= KEY_CHEAK_BOX_LENGTH; i++) {
                CheckBox KeyCheckBox = (CheckBox)TabPage1.Controls["KeyCheckBox" + i];
                KeyCheckBox.CheckedChanged += KeyCheckBoxs_Cheaked;
            }
        }

        private void MenuFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuVersion_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "3DS KeyConverter V1.0\n\n © 2022 あーむ0x64<Twitter:@40414>",
                "バージョン情報",
                MessageBoxButtons.OK);
        }

        /* キーコード変換 */

        // キーラベルと連動してキーコード生成
        private void KeyCheckBoxs_Cheaked(object sender, EventArgs e)
        {
            int keyValue = 0;
            for (int bit = 0; bit < KEY_CHEAK_BOX_LENGTH; bit++) {
                CheckBox KeyCheckBox = (CheckBox)TabPage1.Controls["KeyCheckBox" + (bit + 1)];
                if (KeyCheckBox.Checked) {
                    if (bit <= 11){
                        keyValue += 1 << bit;
                    }
                    // keyが"ZL"か"ZR"だった場合
                    else if (12 <= bit && bit <= 13){
                        keyValue += 1 << (bit + 2);
                    }
                    // keyが"Touch Screen"だった場合
                    else if (bit == 14){
                        keyValue += 1 << (bit + 6);
                    }
                    else{
                        keyValue += 1 << (bit + 9);
                    }
                }
            }
            KeyText.Text = "DD000000 " + keyValue.ToString("X8");
        }

        // キーコードをクリップボードにコピー
        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(KeyText.Text);
        }

        // キーをリセットする
        private void ResetButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= KEY_CHEAK_BOX_LENGTH; i++) {
                CheckBox KeyCheckBox = (CheckBox)TabPage1.Controls["KeyCheckBox" + i];
                KeyCheckBox.Checked = false;
            }
            KeyText.Text = "DD000000 00000000";
        }

        /* キーコード逆変換 */

        // テキストボックスが空の場合、Convertボタンを無効にする
        private void KeyText_Re_TextChanged(object sender, EventArgs e)
        {
            ConvertButton_Re.Enabled = KeyText_Re.Text != "" ? true : false;
        }

        // キーコードを逆変換する
        private void ConvertButton_Re_Click(object sender, EventArgs e)
        {
            // 正しいキーの値か確認
            if (!IsHexString(KeyText_Re.Text)) {
                MessageBox.Show(
                    "16進数を入力してください。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return;
            }
            int keyValue = Convert.ToInt32(KeyText_Re.Text, 16);
            string KeyText = "";

            for (int bit = 0; bit < KEY_CHEAK_BOX_LENGTH; bit++) {
                // 指定されたキーを確認
                if (Convert.ToBoolean(keyValue & 1)) {
                    CheckBox KeyCheckBox = (CheckBox)TabPage1.Controls["KeyCheckBox" + (bit + 1)];
                    KeyText += "(" + KeyCheckBox.Text + ") + ";
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

                if (!Convert.ToBoolean(keyValue)) { break; }
            }

            // 結果を出力
            if (KeyText != "") {
                Output_KeyText_Re.Text = KeyText.Remove(KeyText.Length - 3);
            }
        }

        // キーをリセットする
        private void ResetButton_Re_Click(object sender, EventArgs e)
        {
            KeyText_Re.Text = "00000000";
            Output_KeyText_Re.Text = "";
        }
    }
}