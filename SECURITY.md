# Security Policy

## Scope

This repository is a local technical sample and is not approved for production deployment.

## Local configuration

- Never commit JWT signing keys, passwords, `.env` files, local appsettings overrides, or SQLite databases.
- Supply `Jwt:Key` and `DemoAuth:Password` through .NET User Secrets or environment variables.
- Use values created only for this sample and rotate them if they are exposed.
- Treat every secret present in Git history as permanently public and invalid.

## Reporting a concern

Do not include a suspected secret or exploit details in a public issue. Contact the repository owner through the GitHub profile or use GitHub's private security reporting feature when available.
