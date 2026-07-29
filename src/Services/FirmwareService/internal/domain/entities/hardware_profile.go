package entities

import (
	"errors"
	"fmt"

	"github.com/google/uuid"
)

const MaxNameLength = 128

type HardwareProfile struct {
	id            uuid.UUID
	name          string
	boardRevision string
	isDeprecated  bool
}

func NewHardwareProfile(id uuid.UUID, name string, boardRevision string) (*HardwareProfile, error) {
	if name == "" {
		return nil, errors.New("name can not be empty")
	}

	if len(name) >= MaxNameLength {
		return nil, fmt.Errorf("name should have %c symbols", MaxNameLength)
	}

	if boardRevision == "" {
		return nil, errors.New("boardRevision can not be empty")
	}

	return &HardwareProfile{
		id:            id,
		name:          name,
		boardRevision: boardRevision,
		isDeprecated:  false,
	}, nil
}

func (hp *HardwareProfile) UpdateName(name string) error {
	if name == "" {
		return errors.New("name can not be empty")
	}

	if len(name) >= MaxNameLength {
		return fmt.Errorf("name should have %c symbols", MaxNameLength)
	}

	hp.name = name
	return nil
}

func (hp *HardwareProfile) Deprecate() {
	hp.isDeprecated = true
}

func (hp *HardwareProfile) ID() uuid.UUID         { return hp.id }
func (hp *HardwareProfile) Name() string          { return hp.name }
func (hp *HardwareProfile) BoardRevision() string { return hp.boardRevision }
func (hp *HardwareProfile) IsDeprecated() bool    { return hp.isDeprecated }
