using System;
using System.Windows.Forms;

namespace KeyConverter.Forms
{
    public partial class FormMain : Form
    {
        // ツール情報
        const string GAME_NAME = "3DS KeyConverter";
        const string VERSION = "v1.0.2";
        const string AUTHOR = "アーム";
        const string TWITTER_ID = "@40414";

        // キーボックスの数
        const int KEY_CHEAK_BOX_LENGTH = 23;

        public FormMain()
        {
            InitializeComponent();
        }

        private void MenuFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void KeyConverterForm_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= KEY_CHEAK_BOX_LENGTH; i++) {
                CheckBox keyCheckBox = (CheckBox)TabPage1.Controls[$"KeyCheckBox{i}"];
                keyCheckBox.CheckedChanged += KeyCheckBoxs_Cheaked;
            }
        }

        private void MenuVersion_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"{GAME_NAME} {VERSION}\n\n© 2022 {AUTHOR}<Twitter:{TWITTER_ID}>",
                "バージョン情報",
                MessageBoxButtons.OK);
        }

        /// <summary>
        /// 16進文字列か判定する
        /// </summary>
        /// <returns>16進文字列の場合trueを返す。そうでない場合はfalseを返す。</returns>
        public bool IsHexString(string str)
        {
            if (string.IsNullOrEmpty(str)) {
                return false;
            }
            foreach (char c in str) {
                if (!Uri.IsHexDigit(c)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// キーラベルと連動してキーコード生成
        /// </summary>
        private void KeyCheckBoxs_Cheaked(object sender, EventArgs e)
        {
            int keyValue = 0;
            for (int bit = 0; bit < KEY_CHEAK_BOX_LENGTH; bit++) {
                CheckBox keyCheckBox = (CheckBox)TabPage1.Controls[$"KeyCheckBox{bit + 1}"];
                if (keyCheckBox.Checked) {
                    if (bit <= 11) {
                        keyValue += 1 << bit;
                    }
                    // keyが"ZL"か"ZR"だった場合
                    else if (12 <= bit && bit <= 13) {
                        keyValue += 1 << (bit + 2);
                    }
                    // keyが"Touch Screen"だった場合
                    else if (bit == 14) {
                        keyValue += 1 << (bit + 6);
                    }
                    else{
                        keyValue += 1 << (bit + 9);
                    }
                }
            }
            KeyText.Text = $"DD000000 {keyValue:X8}";
        }

        /// <summary>
        /// キーコードをクリップボードにコピー
        /// </summary>
        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(KeyText.Text);
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
            KeyText.Text = "DD000000 00000000";
        }

        /// <summary>
        /// テキストボックスが空の場合、Convertボタンを無効にする(逆変換)
        /// </summary>
        private void KeyText_Re_TextChanged(object sender, EventArgs e)
        {
            ConvertButton_Re.Enabled = KeyText_Re.Text != "";
        }

        /// <summary>
        /// キーコードを逆変換する
        /// </summary>
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
            string keyText = "";

            for (int bit = 0; bit < KEY_CHEAK_BOX_LENGTH; bit++) {

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
                Output_KeyText_Re.Text = keyText.Remove(keyText.Length - 3);
            }
        }

        /// <summary>
        /// キーをリセットする(逆変換)
        /// </summary>
        private void ResetButton_Re_Click(object sender, EventArgs e)
        {
            KeyText_Re.Text = "00000000";
            Output_KeyText_Re.Text = "";
        }
    }
}