# KeyConverter

**KeyConverter**は、3DSのキーコードを変換・逆変換するためのアプリケーションです。

<div style="display: flex; gap: 10px;">
	<img src="./Docs/Screenshots/app1.png" alt="アプリ画面1" width="45%">
	<img src="./Docs/Screenshots/app2.png" alt="アプリ画面2" width="45%">
</div>

## 特長

- 3DSのキーコードを直感的に変換できます。
- 変換結果をワンクリックでクリップボードにコピー可能。

## キーコードの詳細

以下は、対応する3DSキーコードのリストです。
詳細については、こちらの[参考資料](https://gist.github.com/Nanquitas/d6c920a59c757cf7917c2bffa76de860#file-actionreplaycodetypes-txt)をご覧ください。

```
[KEY_A] = 0x0001,
[KEY_B] = 0x0002,
[KEY_SELECT] = 0x0004,
[KEY_START] = 0x0008,
[KEY_RIGHT] = 0x0010,
[KEY_LEFT] = 0x0020,
[KEY_UP] = 0x0040,
[KEY_DOWN] = 0x0080,
[KEY_R] = 0x0100,
[KEY_L] = 0x0200,
[KEY_X] = 0x0400,
[KEY_Y] = 0x0800,
[KEY_ZL] = 0x4000,
[KEY_ZR] = 0x8000,
[KEY_TOUCH] = 0x100000,
[KEY_CSTICK_RIGHT] = 0x1000000,
[KEY_CSTICK_LEFT] = 0x2000000,
[KEY_CSTICK_UP] = 0x4000000,
[KEY_CSTICK_DOWN] = 0x8000000,
[KEY_CPAD_RIGHT] = 0x10000000,
[KEY_CPAD_LEFT] = 0x20000000,
[KEY_CPAD_UP] = 0x40000000,
[KEY_CPAD_DOWN] = 0x80000000,
```

## ライセンス

本プロジェクトは、MITライセンスの下でライセンスされています。  
詳細については、[LICENSEファイル](./LICENSE)を参照してください。

## 著作権表示

Copyright (C) 2022 ARM

## フィードバック・貢献

バグ報告や機能改善の提案は、ぜひ[Issue](https://github.com/XXXFQ/3ds-KeyConverter/issues)またはプルリクエストでご連絡ください！