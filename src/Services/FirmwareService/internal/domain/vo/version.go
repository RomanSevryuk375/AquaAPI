package vo

import (
	"errors"
	"fmt"
	"regexp"
)

var versionRegexCompiled = regexp.MustCompile(`^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$`)

type Version struct {
	value string
	major int
	minor int
	patch int
}

func NewVersion(major int, minor int, patch int) (*Version, error) {
	if major < 0 || minor < 0 || patch < 0 {
		return nil, errors.New("version parts cannot be negative")
	}

	value := fmt.Sprintf("%d.%d.%d", major, minor, patch)

	if !versionRegexCompiled.MatchString(value) {
		return nil, errors.New("invalid semantic version format")
	}

	return &Version{
		value: value,
		major: major,
		minor: minor,
		patch: patch,
	}, nil
}

func (v *Version) Value() string { return v.value }
func (v *Version) Major() int    { return v.major }
func (v *Version) Minor() int    { return v.minor }
func (v *Version) Patch() int    { return v.patch }
