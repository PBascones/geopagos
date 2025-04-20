-- Drop existing tables if needed
IF EXISTS(SELECT 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID('TournamentResult'))
BEGIN
    ALTER TABLE TournamentResult DROP CONSTRAINT IF EXISTS FK_TournamentResult_Winner;
END;
DROP TABLE IF EXISTS PlayerSnapshot;
DROP TABLE IF EXISTS TournamentResult;

-- Main tournament table
CREATE TABLE TournamentResult (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PlayedDate DATETIME NOT NULL,
    Gender VARCHAR(50) NOT NULL,
    WinnerSnapshotId INT
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
