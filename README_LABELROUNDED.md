# LabelRounded - Control Personalizado

Control de etiqueta profesional con bordes redondeados, colores configurables y soporte para iconos.

## ?? Características

### ? Bordes Redondeados
- `BorderRadius` - Radio de las esquinas (0-50)
- `BorderThickness` - Grosor del borde (0-10)
- `BorderColor` - Color del borde

### ? Colores Personalizables
- `BackgroundColor` - Color de fondo
- `TextColor` - Color del texto
- `ValueColor` - Color del valor (modo dos líneas)
- `IconColor` - Color del icono

### ? Soporte para Iconos
- `Icon` - Imagen del icono
- `IconSize` - Tamaño del icono (8-64)
- `IconSpacing` - Espaciado entre icono y texto
- `IconAlignment` - Posición del icono:
  - `Left` - Izquierda (recomendado para métricas)
  - `Right` - Derecha
  - `Top` - Arriba
  - `Bottom` - Abajo
  - `Center` - Centro

### ? Modo Dos Líneas
Perfecto para mostrar métricas:
- `IsTwoLineMode` - Activar modo título/valor
- `Title` - Texto del título (ej: "TOTAL DEUDA")
- `Value` - Texto del valor (ej: "$1,234.56")
- `TitleFont` - Fuente del título
- `ValueFont` - Fuente del valor

### ? Sombra Opcional
- `ShowShadow` - Mostrar sombra
- `ShadowColor` - Color de la sombra
- `ShadowDepth` - Profundidad (0-10)

## ?? Ejemplos de Uso

### Ejemplo 1: Métrica Simple (Modo Dos Líneas)

```csharp
var lblDebt = new LabelRounded
{
    // Modo dos líneas
    IsTwoLineMode = true,
    Title = "TOTAL DEUDA",
    Value = "$15,432.50",
    
    // Apariencia
    BackgroundColor = Color.White,
    BorderRadius = 12,
    BorderThickness = 0,
    ShowShadow = true,
    ShadowDepth = 3,
    
    // Colores
    TextColor = Color.FromArgb(107, 114, 128),  // Gris para título
    ValueColor = Color.FromArgb(220, 38, 38),    // Rojo para valor
    
    // Tamaño y posición
    Size = new Size(240, 70),
    Location = new Point(20, 100),
    Padding = new Padding(15)
};
```

### Ejemplo 2: Con Icono a la Izquierda

```csharp
var lblBalance = new LabelRounded
{
    // Modo dos líneas con icono
    IsTwoLineMode = true,
    Title = "SALDO",
    Value = "$5,123.45",
    
    // Icono
    Icon = Properties.Resources.dollar_icon,  // Tu imagen
    IconSize = 32,
    IconAlignment = IconAlignment.Left,
    IconSpacing = 12,
    
    // Colores
    BackgroundColor = Color.FromArgb(240, 253, 244),  // Verde claro
    ValueColor = Color.FromArgb(22, 163, 74),         // Verde
    BorderColor = Color.FromArgb(134, 239, 172),      // Verde borde
    BorderThickness = 2,
    BorderRadius = 10,
    
    Size = new Size(250, 80),
    Padding = new Padding(15, 12, 15, 12)
};
```

### Ejemplo 3: Texto Simple con Borde

```csharp
var lblInfo = new LabelRounded
{
    // Modo simple
    IsTwoLineMode = false,
    Text = "Información importante",
    TextAlign = ContentAlignment.MiddleCenter,
    
    // Borde destacado
    BackgroundColor = Color.FromArgb(254, 243, 199),  // Amarillo claro
    BorderColor = Color.FromArgb(245, 158, 11),        // Naranja
    BorderThickness = 3,
    BorderRadius = 8,
    TextColor = Color.FromArgb(146, 64, 14),          // Marrón
    
    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
    Size = new Size(200, 50)
};
```

### Ejemplo 4: Tarjeta con Sombra y Icon Top

```csharp
var lblCard = new LabelRounded
{
    // Icono arriba
    Icon = Properties.Resources.check_circle,
    IconSize = 40,
    IconAlignment = IconAlignment.Top,
    IconSpacing = 10,
    
    // Modo dos líneas
    IsTwoLineMode = true,
    Title = "COMPLETADO",
    Value = "95%",
    
    // Apariencia de tarjeta
    BackgroundColor = Color.White,
    BorderRadius = 15,
    ShowShadow = true,
    ShadowColor = Color.FromArgb(80, 0, 0, 0),
    ShadowDepth = 5,
    
    // Colores
    ValueColor = Color.FromArgb(16, 185, 129),  // Verde
    TextColor = Color.FromArgb(100, 116, 139),   // Gris
    
    Size = new Size(180, 140),
    Padding = new Padding(20)
};
```

## ?? Uso en Panel de Resumen

### Reemplazar los Labels Amarillos

```csharp
// En lugar de:
// lblTotalDebt.BackColor = Color.FromArgb(255, 255, 15);

// Usar:
var lblTotalDebt = new LabelRounded
{
    IsTwoLineMode = true,
    Title = "TOTAL DEUDA",
    Value = "$0.00",
    
    BackgroundColor = Color.White,
    BorderRadius = 10,
    ShowShadow = false,
    
    TitleFont = new Font("Segoe UI", 8F, FontStyle.Regular),
    ValueFont = new Font("Segoe UI", 14F, FontStyle.Bold),
    
    TextColor = Color.FromArgb(107, 114, 128),
    ValueColor = Color.FromArgb(220, 38, 38),  // Rojo
    
    Size = new Size(240, 70),
    Location = new Point(15, 85),
    Padding = new Padding(12, 10, 12, 10)
};

panelSummary.Controls.Add(lblTotalDebt);
```

### Set Completo para Panel de Resumen

```csharp
// Total Deuda (Rojo)
var lblTotalDebt = CreateMetricLabel(
    "TOTAL DEUDA", "$0.00", 
    Color.FromArgb(220, 38, 38), 
    new Point(15, 85));

// Total Pagado (Verde)
var lblTotalPaid = CreateMetricLabel(
    "TOTAL PAGADO", "$0.00", 
    Color.FromArgb(16, 185, 129), 
    new Point(15, 165));

// Saldo Pendiente (Naranja)
var lblBalance = CreateMetricLabel(
    "SALDO PENDIENTE", "$0.00", 
    Color.FromArgb(245, 158, 11), 
    new Point(15, 245));

// Proveedores (Índigo)
var lblSuppliers = CreateMetricLabel(
    "PROVEEDORES", "0", 
    Color.FromArgb(99, 102, 241), 
    new Point(15, 325));

// Helper method
private LabelRounded CreateMetricLabel(string title, string value, Color valueColor, Point location)
{
    return new LabelRounded
    {
        IsTwoLineMode = true,
        Title = title,
        Value = value,
        
        BackgroundColor = Color.White,
        BorderRadius = 8,
        BorderThickness = 0,
        
        TitleFont = new Font("Segoe UI", 8F, FontStyle.Regular),
        ValueFont = new Font("Segoe UI", 14F, FontStyle.Bold),
        
        TextColor = Color.FromArgb(107, 114, 128),
        ValueColor = valueColor,
        
        Size = new Size(240, 70),
        Location = location,
        Padding = new Padding(12, 10, 12, 10)
    };
}
```

## ?? Paleta de Colores Sugerida

### Colores de Fondo
```csharp
Color.White                          // Blanco
Color.FromArgb(248, 250, 252)        // Gris muy claro
Color.FromArgb(241, 245, 249)        // Gris claro
```

### Colores de Valores (Semáforo)
```csharp
Color.FromArgb(220, 38, 38)          // Rojo (Deudas)
Color.FromArgb(16, 185, 129)         // Verde (Pagos)
Color.FromArgb(245, 158, 11)         // Naranja (Pendientes)
Color.FromArgb(99, 102, 241)         // Índigo (Info)
Color.FromArgb(59, 130, 246)         // Azul (Datos)
```

### Colores de Texto
```csharp
Color.FromArgb(15, 23, 42)           // Negro suave
Color.FromArgb(51, 65, 85)           // Gris oscuro
Color.FromArgb(107, 114, 128)        // Gris medio
Color.FromArgb(148, 163, 184)        // Gris claro
```

### Colores de Borde
```csharp
Color.FromArgb(226, 232, 240)        // Gris borde
Color.FromArgb(203, 213, 225)        // Gris borde oscuro
```

## ?? Actualizar Valores Dinámicamente

```csharp
// Actualizar valor
lblTotalDebt.Value = summary.TotalDebt.ToString("C2");

// Cambiar color según condición
if (summary.TotalDebt > 10000)
{
    lblTotalDebt.ValueColor = Color.FromArgb(220, 38, 38);  // Rojo
}
else
{
    lblTotalDebt.ValueColor = Color.FromArgb(34, 197, 94);  // Verde
}
```

## ? Performance

- Usa `DoubleBuffered = true` para evitar parpadeos
- `SmoothingMode.AntiAlias` para bordes suaves
- Optimizado para repintado frecuente

## ?? Compatibilidad

- .NET 8 Windows Forms
- .NET 6/7 Windows Forms
- .NET Framework 4.8 (con ajustes menores)

## ?? Notas

1. **Icono recomendado:** Usar `IconAlignment.Left` para métricas
2. **Tamaño recomendado:** 240x70 para modo dos líneas
3. **BorderRadius óptimo:** Entre 8 y 12 para aspecto moderno
4. **Padding recomendado:** 12-15px para buen espaciado
