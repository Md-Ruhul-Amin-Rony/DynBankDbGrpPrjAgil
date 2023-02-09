ALTER TABLE "public"."bank_transaction" DROP CONSTRAINT "fkey_transaction_to_account";
ALTER TABLE "public"."bank_transaction" DROP CONSTRAINT "fkey_transaction_from_account";
ALTER TABLE "public"."bank_user" DROP CONSTRAINT "fk_user_branch";
ALTER TABLE "public"."bank_account" DROP CONSTRAINT "fk_account_currency";
ALTER TABLE "public"."bank_user" DROP CONSTRAINT "fk_user_role";
ALTER TABLE "public"."bank_account" DROP CONSTRAINT "fk_account_user";
ALTER TABLE "public"."bank_loan" DROP CONSTRAINT "fkey_loan_user";
DROP TABLE IF EXISTS "public"."bank_account";
DROP TABLE IF EXISTS "public"."bank_branch";
DROP TABLE IF EXISTS "public"."bank_currency";
DROP TABLE IF EXISTS "public"."bank_loan";
DROP TABLE IF EXISTS "public"."bank_role";
DROP TABLE IF EXISTS "public"."bank_transaction";
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
CREATE TABLE "public"."bank_loan" ( 
  "id" SERIAL,
  "name" VARCHAR(30) NOT NULL,
  "interest_rate" NUMERIC NOT NULL,
  "user_id" INTEGER NOT NULL,
  CONSTRAINT "bank_loan_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_role" ( 
  "id" SERIAL,
  "name" VARCHAR(20) NOT NULL,
  "is_admin" BOOLEAN NOT NULL,
  "is_client" BOOLEAN NOT NULL,
  CONSTRAINT "bank_role_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_transaction" ( 
  "id" SERIAL,
  "name" VARCHAR(30) NOT NULL,
  "from_account_id" INTEGER NULL,
  "to_account_id" INTEGER NULL,
  --"timestamp" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now() ,--
  "amount" NUMERIC NOT NULL,
  CONSTRAINT "bank_transaction_pkey" PRIMARY KEY ("id")
);
CREATE TABLE "public"."bank_user" ( 
  "id" SERIAL,
  "first_name" VARCHAR(20) NOT NULL,
  "last_name" VARCHAR(20) NOT NULL,
  "email" VARCHAR(50) NOT NULL,
  "pin_code" VARCHAR(4) NOT NULL,
  "role_id" INTEGER NULL,
  "branch_id" INTEGER NULL,
  CONSTRAINT "bank_user_pkey" PRIMARY KEY ("id"),
  CONSTRAINT "unique_email" UNIQUE ("email")
);
ALTER TABLE "public"."bank_account" DISABLE TRIGGER ALL;
ALTER TABLE "public"."bank_branch" DISABLE TRIGGER ALL;
ALTER TABLE "public"."bank_currency" DISABLE TRIGGER ALL;
ALTER TABLE "public"."bank_loan" DISABLE TRIGGER ALL;
ALTER TABLE "public"."bank_role" DISABLE TRIGGER ALL;
ALTER TABLE "public"."bank_transaction" DISABLE TRIGGER ALL;
ALTER TABLE "public"."bank_user" DISABLE TRIGGER ALL;
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Lönekonto', '0', 2, 1, '1000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('ISK', '0', 2, 1, '3000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Pensionsspar', '4', 2, 1, '10000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Förmånskonto', '2', 1, 1, '5000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Lönekonto', '0', 1, 1, '3000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Savings', '1.5', 1, 2, '1000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('ISK', '0', 1, 1, '0');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('Sparkonto', '1.50', 1, 1, '2000');
INSERT INTO "public"."bank_account" ("name", "interest_rate", "user_id", "currency_id", "balance") VALUES ('', '0', 1, 1, '121');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Koala');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Owl');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Panda');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Fox');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Squid');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Lion');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Rabbit');
INSERT INTO "public"."bank_branch" ("name") VALUES ('Tiger');
INSERT INTO "public"."bank_currency" ("name", "exchange_rate") VALUES ('SEK', 1);
INSERT INTO "public"."bank_currency" ("name", "exchange_rate") VALUES ('USD', 10.29);
INSERT INTO "public"."bank_currency" ("name", "exchange_rate") VALUES ('EUR', 10.29);
INSERT INTO "public"."bank_role" ("name", "is_admin", "is_client") VALUES ('Administrator', true, false);
INSERT INTO "public"."bank_role" ("name", "is_admin", "is_client") VALUES ('Client', false, true);
INSERT INTO "public"."bank_role" ("name", "is_admin", "is_client") VALUES ('ClientAdmin', true, true);
INSERT INTO "public"."bank_transaction" ("name", "from_account_id", "to_account_id", "timestamp", "amount") VALUES ('Överföring', 7, 2, '2023-02-07 10:27:25.958896+01', '1100.00');
INSERT INTO "public"."bank_transaction" ("name", "from_account_id", "to_account_id", "timestamp", "amount") VALUES ('Överföring', 7, 2, '2023-02-07 10:27:36.051976+01', '1100.00');
INSERT INTO "public"."bank_transaction" ("name", "from_account_id", "to_account_id", "timestamp", "amount") VALUES ('Överföring', 2, 7, '2023-02-07 10:28:23.190591+01', '500.00');
INSERT INTO "public"."bank_transaction" ("name", "from_account_id", "to_account_id", "timestamp", "amount") VALUES ('Överföring', 7, 2, '2023-02-07 10:55:06.900233+01', '1300.00');
INSERT INTO "public"."bank_user" ("first_name", "last_name", "email", "pin_code", "role_id", "branch_id") VALUES ('Pablo', 'Fransisco P', 'pablo@nej.se', '4567', 2, 1);
INSERT INTO "public"."bank_user" ("first_name", "last_name", "email", "pin_code", "role_id", "branch_id") VALUES ('Krille', 'P', 'krille@hej.se', '1234', 3, 1);
INSERT INTO "public"."bank_user" ("first_name", "last_name", "email", "pin_code", "role_id", "branch_id") VALUES ('reza', 'eskandari', 'reza@chas.se', '1234', 3, 6);
ALTER TABLE "public"."bank_account" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_branch" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_currency" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_loan" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_role" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_transaction" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_user" ENABLE TRIGGER ALL;
ALTER TABLE "public"."bank_account" ADD CONSTRAINT "fk_account_user" FOREIGN KEY ("user_id") REFERENCES "public"."bank_user" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_account" ADD CONSTRAINT "fk_account_currency" FOREIGN KEY ("currency_id") REFERENCES "public"."bank_currency" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_loan" ADD CONSTRAINT "fkey_loan_user" FOREIGN KEY ("user_id") REFERENCES "public"."bank_user" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_transaction" ADD CONSTRAINT "fkey_transaction_to_account" FOREIGN KEY ("to_account_id") REFERENCES "public"."bank_account" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_transaction" ADD CONSTRAINT "fkey_transaction_from_account" FOREIGN KEY ("from_account_id") REFERENCES "public"."bank_account" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_user" ADD CONSTRAINT "fk_user_role" FOREIGN KEY ("role_id") REFERENCES "public"."bank_role" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE "public"."bank_user" ADD CONSTRAINT "fk_user_branch" FOREIGN KEY ("branch_id") REFERENCES "public"."bank_branch" ("id") ON DELETE NO ACTION ON UPDATE NO ACTION;
