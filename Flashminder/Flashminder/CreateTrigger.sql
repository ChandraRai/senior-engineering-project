GO 
BEGIN
IF OBJECT_ID (N'[dbo].[trg_UpdateFlashcard]') IS NOT NULL 
DROP TRIGGER [dbo].[trg_UpdateFlashcard]
END

GO
CREATE TRIGGER trg_UpdateFlashcard ON [dbo].[Flashcard] AFTER UPDATE AS
	UPDATE dbo.Flashcard
	SET ModifiedDate = GETDATE()
	WHERE ID IN (SELECT DISTINCT ID FROM Inserted)
