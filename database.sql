ALTER TABLE "public"."bank_user" DROP CONSTRAINT "fk_user_branch";
ALTER TABLE "public"."bank_account" DROP CONSTRAINT "fk_account_currency";
ALTER TABLE "public"."bank_user" DROP CONSTRAINT "fk_user_role";
ALTER TABLE "public"."bank_account" DROP CONSTRAINT "fk_account_user";
DROP TABLE IF EXISTS "public"."bank_account";
DROP TABLE IF EXISTS "public"."bank_branch";
DROP TABLE IF EXISTS "public"."bank_currency";
DROP TABLE IF EXISTS "public"."bank_role";
DROP TABLE IF EXISTS "public"."bank_user";
CREATE TABLE "public"."bank_account" ( 
  "id" SERIAL,
  "name" VARCHAR(30) NOT NULL,
  "interest_rate" NUMERIC NULL,
  "user_id" INTEGER NOT NULL,
  "currency_id" INTEGER NOT NULL,
  "balance" NUMERIC NOT NULL,
  CONSTRAINT "bank_account_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_branch" ( 
  "id" SERIAL,
  "name" VARCHAR(20) NOT NULL,
  CONSTRAINT "bank_branch_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_currency" ( 
  "id" SERIAL,
  "name" VARCHAR(3) NOT NULL,
  "exchange_rate" DOUBLE PRECISION NOT NULL,
  CONSTRAINT "bank_currency_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_role" ( 
  "id" SERIAL,
  "name" VARCHAR(20) NOT NULL,
  "is_admin" BOOLEAN NOT NULL,
  "is_client" BOOLEAN NOT NULL,
  CONSTRAINT "bank_role_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_user" ( 
  "id" SERIAL,
  "first_name" VARCHAR(20) NOT NULL,
  "last_name" VARCHAR(20) NOT NULL,
  "pin_code" VARCHAR(4) NOT NULL,
  "role_id" INTEGER NULL,
  "branch_id" INTEGER NULL,
  CONSTRAINT "bank_user_pkey" PRIMARY KEY ("id")
);
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Lönekonto', '0', 1, 1, '0');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Stockholm');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Malmö');
INSERT INTO "public"."bank_currency" ("name", "exchange_rate") VALUES ('SEK', 1);
INSERT INTO "public"."bank_currency" ("name", "exchange_rate") VALUES ('USD', 10.29);
INSERT INTO "public"."bank_role" ("name", "is_admin", "is_client") VALUES ('Administrator', true, false);
INSERT INTO "public"."bank_role" ("name", "is_admin", "is_client") VALUES ('Client', false, true);
INSERT INTO "public"."bank_role" ("name", "is_admin", "is_client") VALUES ('ClientAdmin', true, true);
INSERT INTO "public"."bank_user" ("first_name", "last_name", "pin_code", "role_id", "branch_id") VALUES ('Krille', 'P', '1234', 2, 1);
INSERT INTO "public"."bank_user" ("first_name", "last_name", "pin_code", "role_id", "branch_id") VALUES ('Pablo', 'Fransisco P', '4567', 2, 1);
INSERT INTO "public"."bank_user" ("first_name", "last_name", "pin_code", "role_id", "branch_id") VALUES ('Abbe', 'Något', '1234', 2, 1);
ALTER TABLE "public"."bank_account" ADD CONSTRAINT "fk_account_user" FOREIGN KEY ("user_id") REFERENCES "public"."bank_user" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_account" ADD CONSTRAINT "fk_account_currency" FOREIGN KEY ("currency_id") REFERENCES "public"."bank_currency" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_user" ADD CONSTRAINT "fk_user_role" FOREIGN KEY ("role_id") REFERENCES "public"."bank_role" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_user" ADD CONSTRAINT "fk_user_branch" FOREIGN KEY ("branch_id") REFERENCES "public"."bank_branch" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
