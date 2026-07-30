package infrastructure

import (
	"context"
	"fmt"
	"io"
	"time"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/s3"
	"github.com/google/uuid"
)

type S3BlobStorage struct {
	client        *s3.Client
	presignClient *s3.PresignClient
	bucketName    string
}

func NewS3BlobStorage(client *s3.Client, bucketName string) *S3BlobStorage {
	return &S3BlobStorage{
		client:        client,
		presignClient: s3.NewPresignClient(client),
		bucketName:    bucketName,
	}
}

func (s *S3BlobStorage) UploadFile(ctx context.Context, fileStream io.Reader, expectedSizeBytes int) (string, error) {
	storageKey := fmt.Sprintf("firmware/%s.bin", uuid.New().String())

	input := &s3.PutObjectInput{
		Bucket:        aws.String(s.bucketName),
		Key:           aws.String(storageKey),
		Body:          fileStream,
		ContentLength: aws.Int64(int64(expectedSizeBytes)),
		ContentType:   aws.String("application/octet-stream"),
	}

	_, err := s.client.PutObject(ctx, input)
	if err != nil {
		return "", fmt.Errorf("failed to upload file to S3: %w", err)
	}
	return storageKey, nil
}

func (s *S3BlobStorage) GeneratePreSignedURL(ctx context.Context, storageKey string) (string, error) {
	input := &s3.GetObjectInput{
		Bucket: aws.String(s.bucketName),
		Key:    aws.String(storageKey),
	}

	req, err := s.presignClient.PresignGetObject(ctx, input, func(po *s3.PresignOptions) { po.Expires = 15 * time.Minute })
	if err != nil {
		return "", fmt.Errorf("failed to generate pre-signed url: %w", err)
	}
	return req.URL, nil
}
