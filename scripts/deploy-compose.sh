#!/usr/bin/env bash
set -euo pipefail

environment="${1:-}"

if [[ "$environment" != "staging" && "$environment" != "production" ]]; then
  echo "Uso: ./scripts/deploy-compose.sh staging|production"
  exit 1
fi

compose_file="docker-compose.${environment}.yml"

docker compose --env-file .env -f "$compose_file" up -d --build
docker compose --env-file .env -f "$compose_file" ps
