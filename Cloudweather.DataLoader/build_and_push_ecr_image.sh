set -e

# Set variables
AWS_PROFILE="weather-ecr-agent"
AWS_REGION="us-east-1"
ACCOUNT_ID="842206816393"
REPO_NAME="cloud-weather-data-loader"
TAG="latest"
IMAGE_NAME="$REPO_NAME:$TAG"
ECR_URL="$ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$IMAGE_NAME"

# Authenticate Docker with ECR
aws ecr get-login-password --region $AWS_REGION --profile $AWS_PROFILE | \
    docker login --username AWS --password-stdin $ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com

# Build the Docker image
docker build -f ./Dockerfile -t $IMAGE_NAME .

# Tag the Docker image
docker tag $IMAGE_NAME $ECR_URL

# Push the Docker image to ECR
docker push $ECR_URL
