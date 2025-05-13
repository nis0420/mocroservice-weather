#!/usr/bin/env bash
set -euo pipefail                # stop on any error or undefined var

# ───── Configuration ─────────────────────────────────────────────────────
AWS_PROFILE=${AWS_PROFILE:-weather-ecr-agent}
AWS_REGION="us-east-2"
ACCOUNT_ID="842206816393"
REPO_NAME="cloud-weather-report"    # ← report repo

TAG="${1:-latest}"               # optional CLI arg overrides the tag
IMAGE_NAME="$REPO_NAME:$TAG"
REGISTRY="$ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com"
ECR_URL="$REGISTRY/$IMAGE_NAME"

# Increase time-outs (helps behind slow or proxy links)
export DOCKER_CLIENT_TIMEOUT=300
export COMPOSE_HTTP_TIMEOUT=300

# ───── Authenticate to ECR ──────────────────────────────────────────────
echo ">> Logging in to ECR ($AWS_REGION) ..."
docker login -u AWS \
  -p "$(aws ecr get-login-password --region "$AWS_REGION" --profile "$AWS_PROFILE")" \
  "$REGISTRY"

# ───── Build, tag, push ─────────────────────────────────────────────────
echo ">> Building Docker image $IMAGE_NAME ..."
docker build -f Dockerfile -t "$IMAGE_NAME" .

echo ">> Tagging image with ECR URI ..."
docker tag "$IMAGE_NAME" "$ECR_URL"

echo ">> Pushing $ECR_URL ..."
docker push "$ECR_URL"

echo "✓ Image pushed successfully."