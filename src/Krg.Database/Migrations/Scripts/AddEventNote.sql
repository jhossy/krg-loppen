BEGIN TRANSACTION;
ALTER TABLE [Event] ADD [Note] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250803143210_AddEventNote', N'9.0.0');

COMMIT;
GO

