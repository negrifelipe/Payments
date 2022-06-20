CREATE TABLE [dbo].[PaymentItems] (
    [Id]        INT              IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (127)   NOT NULL,
    [Amount]    INT              DEFAULT 1 NOT NULL,
    [Price]     DECIMAL (15, 2)  NOT NULL,
    [PaymentId] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PaymentItems_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[Payments] ([Id])
);

