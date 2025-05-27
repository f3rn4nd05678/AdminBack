
-- =========================
-- FASE 1: FUNDAMENTOS
-- =========================

-- Roles de usuario
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    descripcion TEXT
);

-- Usuarios del sistema
CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    nombre_completo VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    contrasena TEXT NOT NULL,
    activo BOOLEAN DEFAULT TRUE,
    fecha_creacion TIMESTAMP DEFAULT NOW(),
    rol_id INTEGER REFERENCES roles(id) ON DELETE SET NULL
);

-- Configuraciones generales del sistema
CREATE TABLE configuracion_sistema (
    id SERIAL PRIMARY KEY,
    clave VARCHAR(50) UNIQUE NOT NULL,
    valor TEXT NOT NULL,
    descripcion TEXT
);

-- =========================
-- FASE 2: CATALOGOS
-- =========================

-- Almacenes
CREATE TABLE almacenes (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    ubicacion VARCHAR(150),
    descripcion TEXT,
    activo BOOLEAN DEFAULT TRUE
);

-- Productos
CREATE TABLE productos (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(50) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    unidad_medida VARCHAR(10),
    precio_unitario DECIMAL(12,2),
    activo BOOLEAN DEFAULT TRUE
);

-- Clientes
CREATE TABLE clientes (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    rfc VARCHAR(20),
    direccion TEXT,
    email VARCHAR(100),
    telefono VARCHAR(20),
    activo BOOLEAN DEFAULT TRUE
);

-- Proveedores
CREATE TABLE proveedores (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    rfc VARCHAR(20),
    direccion TEXT,
    email VARCHAR(100),
    telefono VARCHAR(20),
    activo BOOLEAN DEFAULT TRUE
);

-- =========================
-- FASE 3: INVENTARIO
-- =========================

-- Entradas de inventario
CREATE TABLE entradas_inventario (
    id SERIAL PRIMARY KEY,
    producto_id INTEGER NOT NULL REFERENCES productos(id) ON DELETE CASCADE,
    almacen_id INTEGER NOT NULL REFERENCES almacenes(id) ON DELETE CASCADE,
    cantidad DECIMAL(12,2) NOT NULL,
    fecha_entrada TIMESTAMP DEFAULT NOW(),
    referencia TEXT,
    usuario_id INTEGER REFERENCES usuarios(id)
);

-- Salidas de inventario
CREATE TABLE salidas_inventario (
    id SERIAL PRIMARY KEY,
    producto_id INTEGER NOT NULL REFERENCES productos(id) ON DELETE CASCADE,
    almacen_id INTEGER NOT NULL REFERENCES almacenes(id) ON DELETE CASCADE,
    cantidad DECIMAL(12,2) NOT NULL,
    fecha_salida TIMESTAMP DEFAULT NOW(),
    referencia TEXT,
    usuario_id INTEGER REFERENCES usuarios(id)
);

-- Stock actual por producto y almacén
CREATE TABLE inventario_actual (
    id SERIAL PRIMARY KEY,
    producto_id INTEGER NOT NULL REFERENCES productos(id) ON DELETE CASCADE,
    almacen_id INTEGER NOT NULL REFERENCES almacenes(id) ON DELETE CASCADE,
    cantidad DECIMAL(12,2) NOT NULL,
    UNIQUE (producto_id, almacen_id)
);
