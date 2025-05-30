
-- =========================
-- FASE 1: FUNDAMENTOS
-- =========================

-- Roles de usuario
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    descripcion TEXT
);

INSERT INTO roles (id, nombre, descripcion) VALUES
(1, 'Administrador', 'Administrador del sistema'),
(2, 'ResponsableBodega', 'Responsable de Bodega'),
(3, 'EncargadoCompras', 'Encargado de Compras'),
(4, 'GestorVentas', 'Gestor de Ventas'),
(5, 'CoordinadorLogistica', 'Coordinador de Logística'),
(6, 'AnalistaContable', 'Analista Contable'),
(7, 'Cliente', 'Cliente del sistema'),
(8, 'Proveedor', 'Proveedor del sistema');

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

select * from clientes;

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


-- Tabla de módulos
CREATE TABLE modulos (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

-- Tabla de acciones
CREATE TABLE acciones (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    modulo_id INT REFERENCES modulos(id) ON DELETE CASCADE
);

-- Permisos por rol
CREATE TABLE permisos (
    id SERIAL PRIMARY KEY,
    rol_id INT REFERENCES roles(id) ON DELETE CASCADE,
    accion_id INT REFERENCES acciones(id) ON DELETE CASCADE
);



INSERT INTO modulos (nombre) VALUES
('Gestión del Sistema'),
('Inventario'),
('Compras'),
('Ventas'),
('Logística'),
('Finanzas'),
('Cliente'),
('Proveedor');


-- Gestión del Sistema
INSERT INTO acciones (nombre, modulo_id) VALUES
('Gestionar usuarios y roles', 1),
('Configurar parámetros del sistema', 1);

-- Inventario
INSERT INTO acciones (nombre, modulo_id) VALUES
('Registrar entrada de mercancía', 2),
('Registrar salida de mercancía', 2),
('Realizar inventario físico', 2),
('Consultar stock en tiempo real', 2);

-- Compras
INSERT INTO acciones (nombre, modulo_id) VALUES
('Crear orden de compra', 3),
('Evaluar proveedor', 3),
('Aprobar orden de compra', 3);

-- Ventas
INSERT INTO acciones (nombre, modulo_id) VALUES
('Registrar pedido de cliente', 4),
('Consultar historial de pedidos', 4),
('Emitir factura de venta', 4);

-- Logística
INSERT INTO acciones (nombre, modulo_id) VALUES
('Planificar ruta de entrega', 5),
('Asignar transportista', 5),
('Monitorear entrega en tránsito', 5);

-- Finanzas
INSERT INTO acciones (nombre, modulo_id) VALUES
('Registrar pago a proveedor', 6),
('Registrar cobro de cliente', 6),
('Generar reporte financiero', 6);

-- Cliente
INSERT INTO acciones (nombre, modulo_id) VALUES
('Visualizar estado de pedido', 7),
('Descargar factura', 7);

-- Proveedor
INSERT INTO acciones (nombre, modulo_id) VALUES
('Confirmar recepción de orden', 8),
('Notificar envío de mercancía', 8);



-- ADMINISTRADOR
INSERT INTO permisos (rol_id, accion_id)
SELECT 1, id FROM acciones
WHERE nombre IN (
    'Gestionar usuarios y roles',
    'Configurar parámetros del sistema'
);

-- RESPONSABLE DE BODEGA
INSERT INTO permisos (rol_id, accion_id)
SELECT 2, id FROM acciones
WHERE nombre IN (
    'Registrar entrada de mercancía',
    'Registrar salida de mercancía',
    'Realizar inventario físico',
    'Consultar stock en tiempo real'
);

-- ENCARGADO DE COMPRAS
INSERT INTO permisos (rol_id, accion_id)
SELECT 3, id FROM acciones
WHERE nombre IN (
    'Crear orden de compra',
    'Evaluar proveedor',
    'Aprobar orden de compra'
);

-- GESTOR DE VENTAS
INSERT INTO permisos (rol_id, accion_id)
SELECT 4, id FROM acciones
WHERE nombre IN (
    'Registrar pedido de cliente',
    'Consultar historial de pedidos',
    'Emitir factura de venta'
);

-- COORDINADOR DE LOGÍSTICA
INSERT INTO permisos (rol_id, accion_id)
SELECT 5, id FROM acciones
WHERE nombre IN (
    'Planificar ruta de entrega',
    'Asignar transportista',
    'Monitorear entrega en tránsito'
);

-- ANALISTA CONTABLE
INSERT INTO permisos (rol_id, accion_id)
SELECT 6, id FROM acciones
WHERE nombre IN (
    'Registrar pago a proveedor',
    'Registrar cobro de cliente',
    'Generar reporte financiero'
);

-- CLIENTE
INSERT INTO permisos (rol_id, accion_id)
SELECT 7, id FROM acciones
WHERE nombre IN (
    'Visualizar estado de pedido',
    'Descargar factura'
);

-- PROVEEDOR
INSERT INTO permisos (rol_id, accion_id)
SELECT 8, id FROM acciones
WHERE nombre IN (
    'Confirmar recepción de orden',
    'Notificar envío de mercancía'
);




CREATE OR REPLACE FUNCTION sp_obtener_menu_rol_jerarquico(p_rol_id INT)
RETURNS JSON AS
$$
BEGIN
  IF p_rol_id = 1 THEN
    -- Admin: devolver todo
    RETURN (
      SELECT json_agg(json_build_object(
        'modulo', m.nombre,
        'acciones', (
          SELECT json_agg(a.nombre)
          FROM acciones a
          WHERE a.modulo_id = m.id
        )
      ))
      FROM modulos m
      WHERE EXISTS (
        SELECT 1 FROM acciones a WHERE a.modulo_id = m.id
      )
    );
  ELSE
    -- Otros roles: devolver solo los permitidos
    RETURN (
      SELECT json_agg(json_build_object(
        'modulo', m.nombre,
        'acciones', (
          SELECT json_agg(a.nombre)
          FROM acciones a
          JOIN permisos p ON p.accion_id = a.id
          WHERE a.modulo_id = m.id AND p.rol_id = p_rol_id
        )
      ))
      FROM modulos m
      WHERE EXISTS (
        SELECT 1
        FROM acciones a
        JOIN permisos p ON p.accion_id = a.id
        WHERE a.modulo_id = m.id AND p.rol_id = p_rol_id
      )
    );
  END IF;
END;
$$ LANGUAGE plpgsql;



INSERT INTO configuracion_sistema (clave, valor, descripcion) VALUES
('moneda', 'Quetzales', 'Moneda oficial del sistema (GTQ)'),
('stock_minimo_alerta', '10', 'Nivel de stock mínimo antes de mostrar alerta'),
('dias_maximos_entrega', '5', 'Días máximos para entrega de pedidos'),
('correo_soporte', 'soporte@empresa.com', 'Correo electrónico de soporte'),
('formato_fecha', 'dd/MM/yyyy', 'Formato estándar de fecha'),
('mostrar_precios_con_iva', 'true', 'Determina si los precios se muestran con IVA'),
('habilitar_edicion_ventas', 'false', 'Permitir edición de ventas emitidas'),
('porcentaje_iva', '12', 'Porcentaje de IVA aplicable'),
('stock_reservado_automatico', 'true', 'Reserva automática de stock en pedidos'),
('version_sistema', '1.0.0', 'Versión actual del sistema');



-----------------ESCRIPTS 27/05
INSERT INTO acciones (nombre, modulo_id) VALUES
('Gestionar productos', 2),
('Gestionar almacenes', 2);


-- Responsable de Bodega (si se permite)
INSERT INTO permisos (rol_id, accion_id)
SELECT 2, id FROM acciones WHERE nombre IN ('Gestionar productos', 'Gestionar almacenes');


-----------------ESCRIPTS 28/05
ALTER TABLE proveedores ADD COLUMN catalogo_url TEXT;
ALTER TABLE proveedores ADD COLUMN track_url TEXT;


CREATE TABLE orden_compra (
    id SERIAL PRIMARY KEY,
    proveedor_id INT NOT NULL REFERENCES proveedores(id),
    fecha_creacion TIMESTAMP DEFAULT NOW(),
    estado VARCHAR(20) DEFAULT 'Pendiente',
    total_estimado DECIMAL(12,2),
    enviada BOOLEAN DEFAULT FALSE
);

CREATE TABLE detalle_orden_compra (
    id SERIAL PRIMARY KEY,
    orden_id INT NOT NULL REFERENCES orden_compra(id) ON DELETE CASCADE,
    producto_id INT NOT NULL REFERENCES productos(id),
    cantidad DECIMAL(12,2) NOT NULL,
    precio_unitario DECIMAL(12,2) NOT NULL
);

ALTER TABLE proveedores ADD COLUMN orden_url TEXT;


CREATE TABLE pedido_cliente (
    id SERIAL PRIMARY KEY,
    cliente_id INT NOT NULL REFERENCES clientes(id),
    fecha TIMESTAMP DEFAULT NOW(),
    estado VARCHAR(20) DEFAULT 'Pendiente',
    total DECIMAL(12,2)
);


CREATE TABLE detalle_pedido_cliente (
    id SERIAL PRIMARY KEY,
    pedido_id INT NOT NULL REFERENCES pedido_cliente(id) ON DELETE CASCADE,
    producto_id INT NOT NULL REFERENCES productos(id),
    cantidad DECIMAL(12,2) NOT NULL,
    precio_unitario DECIMAL(12,2) NOT NULL
);

ALTER TABLE pedido_cliente ADD COLUMN factura_id_mongo TEXT;


-----------------ESCRIPTS 29/05
CREATE TABLE pagos_cliente (
    id SERIAL PRIMARY KEY,
    pedido_id INT NOT NULL REFERENCES pedido_cliente(id),
    monto DECIMAL(12,2) NOT NULL,
    fecha_pago TIMESTAMP DEFAULT NOW(),
    referencia TEXT
);


CREATE TABLE notas_credito (
    id SERIAL PRIMARY KEY,
    pedido_id INT NOT NULL REFERENCES pedido_cliente(id),
    monto DECIMAL(12,2) NOT NULL,
    motivo TEXT NOT NULL,
    fecha TIMESTAMP DEFAULT NOW()
);


CREATE TABLE pagos_proveedor (
    id SERIAL PRIMARY KEY,
    orden_id INT NOT NULL REFERENCES orden_compra(id),
    monto DECIMAL(12,2) NOT NULL,
    fecha_pago TIMESTAMP DEFAULT NOW(),
    referencia TEXT
);


CREATE TABLE transportistas (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100),
    telefono VARCHAR(20),
    activo BOOLEAN DEFAULT TRUE
);


CREATE TABLE ruta_entrega (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100),
    descripcion TEXT,
    fecha TIMESTAMP DEFAULT NOW()
);

CREATE TABLE entrega_pedido (
    id SERIAL PRIMARY KEY,
    pedido_id INT REFERENCES pedido_cliente(id),
    transportista_id INT REFERENCES transportistas(id),
    ruta_id INT REFERENCES ruta_entrega(id),
    estado VARCHAR(30) DEFAULT 'Pendiente',
    fecha_asignacion TIMESTAMP DEFAULT NOW()
);













