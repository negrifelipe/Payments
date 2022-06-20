CREATE TABLE [dbo].[Payments] (
    [Id]         UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL,
    [Currency]   VARCHAR (3)      NOT NULL,
    [State]      NVARCHAR (MAX)   DEFAULT 'Waiting' NOT NULL,
    [Provider]   NVARCHAR (MAX)   NOT NULL,
    [CreateDate] DATETIME         DEFAULT GETUTCDATE() NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

