CREATE TABLE hardware_profiles (
    id UUID PRIMARY KEY,
    name VARCHAR(128) NOT NULL,
    board_revision VARCHAR(64) NOT NULL,
    is_deprecated BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE firmwares (
    id UUID PRIMARY KEY,
    hardware_profile_id UUID NOT NULL,
    version VARCHAR(64) NOT NULL,
    file_hash CHAR(64) NOT NULL,    
    size_bytes BIGINT NOT NULL,     
    storage_key VARCHAR(256) NOT NULL,
    status INT NOT NULL DEFAULT 0,  
    revoke_reason TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    
    CONSTRAINT fk_firmwares_hardware_profile_id 
        FOREIGN KEY (hardware_profile_id) 
        REFERENCES hardware_profiles (id) 
        ON DELETE RESTRICT
);

CREATE INDEX ix_firmwares_hardware_profile_id ON firmwares (hardware_profile_id);
CREATE UNIQUE INDEX ix_firmwares_hw_version ON firmwares (hardware_profile_id, version);

CREATE TABLE rollout_campaigns (
    id UUID PRIMARY KEY,
    firmware_id UUID NOT NULL,
    name VARCHAR(128) NOT NULL,
    status INT NOT NULL DEFAULT 0, 
    cancel_reason TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_rollout_campaigns_firmware_id 
        FOREIGN KEY (firmware_id) 
        REFERENCES firmwares (id) 
        ON DELETE RESTRICT
);

CREATE TABLE rollout_targets (
    id UUID PRIMARY KEY,
    campaign_id UUID NOT NULL,
    controller_id UUID NOT NULL,
    status INT NOT NULL DEFAULT 0,  
    error_message TEXT,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_rollout_targets_campaign_id 
        FOREIGN KEY (campaign_id) 
        REFERENCES rollout_campaigns (id) 
        ON DELETE CASCADE
);

CREATE INDEX ix_rollout_targets_controller_id ON rollout_targets (controller_id);
CREATE INDEX ix_rollout_campaigns_status ON rollout_campaigns (status);
CREATE UNIQUE INDEX ix_rollout_targets_campaign_controller ON rollout_targets (campaign_id, controller_id);