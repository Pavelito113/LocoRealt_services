USE LocoDB;
GO

-- Основная информация о таблицах, первичных ключах и внешних связях
WITH PrimaryKeys AS (
    SELECT 
        t.name AS TableName,
        c.name AS ColumnName
    FROM sys.key_constraints kc
    INNER JOIN sys.index_columns ic ON kc.unique_index_id = ic.index_id AND kc.parent_object_id = ic.object_id
    INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
    INNER JOIN sys.tables t ON kc.parent_object_id = t.object_id
    WHERE kc.type = 'PK'
),
ForeignKeys AS (
    SELECT 
        fk.name AS FK_Name,
        tp.name AS ParentTable,
        cp.name AS ParentColumn,
        tr.name AS RefTable,
        cr.name AS RefColumn
    FROM sys.foreign_keys fk
    INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    INNER JOIN sys.tables tp ON fkc.parent_object_id = tp.object_id
    INNER JOIN sys.columns cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
    INNER JOIN sys.tables tr ON fkc.referenced_object_id = tr.object_id
    INNER JOIN sys.columns cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
)
-- Формируем вывод
SELECT 
    t.name AS TableName,
    c.name AS ColumnName,
    ty.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable,
    CASE WHEN pk.ColumnName IS NOT NULL THEN 'PK' ELSE '' END AS PrimaryKey,
    fk.FK_Name,
    fk.RefTable,
    fk.RefColumn
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
LEFT JOIN PrimaryKeys pk ON t.name = pk.TableName AND c.name = pk.ColumnName
LEFT JOIN ForeignKeys fk ON t.name = fk.ParentTable AND c.name = fk.ParentColumn
ORDER BY t.name, c.column_id;
