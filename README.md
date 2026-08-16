# Sentinel System

[![Build](https://github.com/SergioKostyukov/Sentinel/actions/workflows/build.yml/badge.svg)](https://github.com/SergioKostyukov/Sentinel/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/github/license/SergioKostyukov/Sentinel)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)](docker-compose.yml)
[![Top language](https://img.shields.io/github/languages/top/SergioKostyukov/Sentinel)](https://github.com/SergioKostyukov/Sentinel)
[![Repo size](https://img.shields.io/github/repo-size/SergioKostyukov/Sentinel)](https://github.com/SergioKostyukov/Sentinel)
[![Last commit](https://img.shields.io/github/last-commit/SergioKostyukov/Sentinel)](https://github.com/SergioKostyukov/Sentinel/commits/main)

Система для централізованого моніторингу статусу сервісів та збору логів.

## 📚 Documentation

| Document | Description |
| --- | --- |
| `/docs/SentinelSchemes.drawio` | Файл схеми проекту |
| [Project Contributing](docs/CONTRIBUTING.md) | Правила стилю комітів |

---

## 🧱 Architecture

![Architecture](docs/architecture.png)

---

## 📁 Project structure

```
├── SentinelApi.Identity/
├── SentinelApi.Monitoring/
├── SentinelLib.Identity.Security/
├── SentinelLib.Monitoring.SDK/
├── SentinelLib.Monitoring.Logging.NLog/
├── SentinelUI/
├── docs/
├── .gitignore
├── README.md
```

---

## 📚 Services Documentation

| Service | Description | Documentation |
| --- | --- | --- |
| **SentinelApi.Identity** | *Аутентифікація користувачів* | [README](SentinelApi.Identity/README.md) |
| **SentinelApi.Monitoring** | *Конфігурація та запуск перевірок, збирання логів* | [README](SentinelApi.Monitoring/README.md) |
| **SentinelLib.Identity.Security** | *Бібліотека верифікації токена авторизації* | [README](SentinelLib.Identity.Security/README.md) |
| **SentinelLib.Monitoring.SDK** | *Бібліотека SDK для моніторингу* | [README](SentinelLib.Monitoring.SDK/README.md) |
| **SentinelLib.Monitoring.Logging.NLog** | *Бібліотека логування для моніторингу* | [README](SentinelLib.Monitoring.Logging.NLog/README.md) |
| **SentinelUI** | *Клієнтський інтерфейс для взаємодії користувача з системою* | [README](SentinelUI/README.md) |
