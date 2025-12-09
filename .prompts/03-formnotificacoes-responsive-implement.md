# Implementation Prompt: FormNotificacoes Responsive Layout

## Objective
Implement responsive layout changes to FormNotificacoes.cs following the implementation plan from Stage 02.

## Input
Receives `<implementation-plan>` from Stage 02 containing:
- Ordered implementation steps
- Exact code changes for each component
- Verification criteria
- Testing checklist

## Implementation Instructions

### File to Modify
`06_bibliotecaJK/Forms/FormNotificacoes.cs`

### Implementation Rules

1. **Follow the plan exactly** - Do not deviate from specified changes
2. **One section at a time** - Complete each step before moving to next
3. **Preserve all logic** - Do not modify any method bodies except InitializeComponent()
4. **Test incrementally** - Verify each change compiles before proceeding

### Required Changes Summary

#### Form Properties (lines ~46-50)
```csharp
// ADD these properties
this.MinimumSize = new Size(800, 500);
this.AutoScroll = true;
```

#### Header Panel (pnlHeader)
```csharp
// REPLACE fixed positioning with Dock
var pnlHeader = new Panel
{
    Dock = DockStyle.Top,
    Height = 80,
    BackColor = Color.FromArgb(63, 81, 181)
};

// UPDATE lblTitulo anchor
var lblTitulo = new Label
{
    Text = "CENTRAL DE NOTIFICACOES",
    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
    ForeColor = Color.White,
    Location = new Point(20, 15),
    AutoSize = true,
    Anchor = AnchorStyles.Top | AnchorStyles.Left
};
```

#### Filter Panel (pnlFiltros)
```csharp
// REPLACE with Dock and add padding
var pnlFiltros = new Panel
{
    Dock = DockStyle.Top,
    Height = 60,
    BackColor = Color.White,
    BorderStyle = BorderStyle.FixedSingle,
    Padding = new Padding(10)
};

// Anchor btnAtualizar to right
btnAtualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
```

#### DataGridView (dgvNotificacoes)
```csharp
// REPLACE fixed positioning with Dock.Fill
dgvNotificacoes = new DataGridView
{
    Dock = DockStyle.Fill,
    // ... keep all other properties
};
```

#### Action Panel (pnlAcoes)
```csharp
// REPLACE with Dock.Bottom and FlowLayoutPanel for buttons
var pnlAcoes = new Panel
{
    Dock = DockStyle.Bottom,
    Height = 60,
    BackColor = Color.White,
    BorderStyle = BorderStyle.FixedSingle
};

// Create FlowLayoutPanel for action buttons
var flowButtons = new FlowLayoutPanel
{
    Dock = DockStyle.Left,
    AutoSize = true,
    FlowDirection = FlowDirection.LeftToRight,
    WrapContents = false,
    Padding = new Padding(5)
};

// Add buttons to FlowLayoutPanel
flowButtons.Controls.Add(btnMarcarLida);
flowButtons.Controls.Add(btnMarcarTodasLidas);
flowButtons.Controls.Add(btnExcluir);

pnlAcoes.Controls.Add(flowButtons);

// Anchor btnFechar to right
btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
btnFechar.Location = new Point(pnlAcoes.Width - 110, 12);
```

### Control Addition Order (Critical for Dock)
When using Dock, controls must be added in reverse visual order:
1. Add pnlAcoes first (Dock.Bottom)
2. Add dgvNotificacoes second (Dock.Fill)
3. Add pnlFiltros third (Dock.Top)
4. Add pnlHeader last (Dock.Top)

### Preserved Functionality Verification
After implementation, verify these still work:
- [ ] Filters change displayed data
- [ ] "Marcar como Lida" button works
- [ ] "Marcar Todas como Lidas" button works with confirmation
- [ ] "Excluir" button works with confirmation
- [ ] Auto-refresh every 30 seconds
- [ ] Priority colors (URGENTE=red, ALTA=orange, NORMAL=yellow, BAIXA=green)
- [ ] Selection enables/disables buttons

## Output Requirements

Produce the complete modified FormNotificacoes.cs file with:
1. All responsive layout changes applied
2. All existing functionality preserved
3. Clean, compilable code
4. No hardcoded widths dependent on form size

## Build Validation
After implementation, run:
```bash
cd 06_bibliotecaJK && dotnet build BibliotecaJK.csproj
```

Expected result: Build succeeded with 0 errors.

## Success Criteria
- [ ] Form has MinimumSize of 800x500
- [ ] Form has AutoScroll enabled
- [ ] Header stretches full width on resize
- [ ] DataGridView fills available space
- [ ] Action buttons flow correctly
- [ ] Close button stays anchored to right
- [ ] All business logic preserved
- [ ] Build completes successfully
