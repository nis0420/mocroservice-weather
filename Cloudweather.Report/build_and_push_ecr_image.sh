#!/usr/bin/env bash
set -euo pipefail                       # stop on any error or unset var

# ───── Variables ────────────────────────────────────────────────────
AWS_PROFILE=${AWS_PROFILE:-weather-ecr-agent}
AWS_REGION="us-east-2"                  # ← region that hosts the repo
ACCOUNT_ID="842206816393"
REPO_NAME="cloud-weather-report"

TAG="${1:-latest}"                      # optional CLI arg overrides tag
IMAGE_NAME="$REPO_NAME:$TAG"
REGISTRY="$ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com"
ECR_URL="$REGISTRY/$IMAGE_NAME"         # full URI <acct>.dkr.ecr.<region>.amazonaws.com/repo:tag  :contentReference[oaicite:1]{index=1}

# (Optional) raise time-outs if your proxy is flaky
export DOCKER_CLIENT_TIMEOUT=300
export COMPOSE_HTTP_TIMEOUT=300
# unset HTTP_PROXY HTTPS_PROXY           # uncomment if proxy breaks uploads

# ───── Authenticate to ECR ─────────────────────────────────────────
echo ">> Logging in to ECR ($AWS_REGION) ..."
docker login -u AWS \
  -p "$(aws ecr get-login-password --region "$AWS_REGION" --profile "$AWS_PROFILE")" \
  "$REGISTRY"

# ───── Build, tag, push ────────────────────────────────────────────
echo ">> Building $IMAGE_NAME ..."
docker build -f Dockerfile -t "$IMAGE_NAME" .

echo ">> Tagging image with ECR URI ..."
docker tag "$IMAGE_NAME" "$ECR_URL"

echo ">> Pushing $ECR_URL ..."
docker push "$ECR_URL"                 # pushes all layers; tag ‘latest’ by default

echo "✓ Image pushed successfully."
