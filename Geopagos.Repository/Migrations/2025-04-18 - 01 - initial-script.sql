-- Drop existing tables if needed
DROP TABLE IF EXISTS PlayerSnapshot;
DROP TABLE IF EXISTS TournamentResult;

-- Main tournament table
CREATE TABLE TournamentResult (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PlayedDate DATETIME NOT NULL,
    Gender VARCHAR(50) NOT NULL,
    WinnerSnapshotId INT NOT NULL
);

-- Related snapshots of players
CREATE TABLE PlayerSnapshot (
    Id INT PRIMARY KEY IDENTITY(1,1),
    [Name] VARCHAR(100) NOT NULL,
    SkillLevel INT NOT NULL,
    Gender VARCHAR(50) NOT NULL,
    Strength INT,
    Speed INT,
    ReactionTime INT,
    TournamentResultId INT NOT NULL,
    FOREIGN KEY (TournamentResultId) REFERENCES TournamentResult(Id)
);

-- Add FK to winner snapshot
ALTER TABLE TournamentResult
ADD CONSTRAINT FK_TournamentResult_Winner
FOREIGN KEY (WinnerSnapshotId) REFERENCES PlayerSnapshot(Id);
