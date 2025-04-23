
-- ============================
-- Insert 10,000 parent todos
-- ============================
INSERT INTO "TodoItems" ("Title", "IsDone", "ParentId")
SELECT
  'Todo #' || g AS "Title",
  false AS "IsDone",
  NULL AS "ParentId"
FROM generate_series(1, 10000000) g;

-- ============================
-- Insert 10 comments per todo (100,000 total)
-- ============================
INSERT INTO "Comments" ("Content", "CreatedAt", "TodoItemId")
SELECT
  'Comment #' || c.n || ' on Todo ' || t."Id" AS "Content",
  NOW() - (c.n || ' minutes')::interval AS "CreatedAt",
  t."Id" AS "TodoItemId"
FROM (
  SELECT "Id" FROM "TodoItems" WHERE "ParentId" IS NULL
) t,
generate_series(1, 1) AS c(n);