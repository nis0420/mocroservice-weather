CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250418153132_init') THEN
    CREATE TABLE temprature (
        "Id" uuid NOT NULL,
        "TempHighF" numeric NOT NULL,
        "TempLowF" numeric NOT NULL,
        "CreatedOn" timestamp with time zone NOT NULL,
        "ZipCode" text NOT NULL,
        CONSTRAINT "PK_temprature" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250418153132_init') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250418153132_init', '9.0.4');
    END IF;
END $EF$;
COMMIT;

