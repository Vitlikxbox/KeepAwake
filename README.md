<div align="center">

# ☕ KeepAwake

**Не даёт Windows уснуть: для загрузок, рендера, презентаций и долгих сборок**

![Windows](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?logo=windows&logoColor=white)
![C#](https://img.shields.io/badge/C%23-.NET%20Framework%204-512BD4?logo=dotnet&logoColor=white)
![No install](https://img.shields.io/badge/установка-не%20нужна-success)
![License](https://img.shields.io/badge/license-MIT-blue)

<img src="docs/screenshot.png" alt="Скриншот" width="300">

</div>

## ✨ Возможности

- 🛌 Блокирует спящий режим, пока включён. Никакого «дёргания мыши», используется официальный API Windows (`SetThreadExecutionState`)
- 🖥️ По желанию не даёт гаснуть экрану
- ⏱️ Таймер: 15 минут, 30 минут, 1, 2, 4 или 8 часов, либо бессрочно
- 📥 Сворачивается в трей, управляется правой кнопкой по иконке
- 🔁 После выключения или закрытия программы настройки питания сразу возвращаются как были

## 💡 Когда пригодится

- Качаешь большой файл или игру
- Рендеришь видео или компилируешь проект
- Показываешь презентацию или смотришь лекцию

## 📥 Установка

1. Скачай `KeepAwake.exe` из раздела [**Releases**](../../releases/latest)
2. Запусти его, установка не нужна

## 🛠️ Сборка из исходников

Нужен только Windows: компилятор C# входит в состав .NET Framework.

```bat
build.bat
```

## 📄 Лицензия

[MIT](LICENSE)
