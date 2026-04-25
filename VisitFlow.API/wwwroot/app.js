
/* VisitFlow — app.js */
const API = '/api';

// Estado global
let token = null;
let currentUser = null;

// ─── HTTP ────────────────────────────────────────────────────────────────────
async function http(method, path, body) {
  const headers = { 'Content-Type': 'application/json' };
  if (token) headers['Authorization'] = 'Bearer ' + token;
  const res = await fetch(API + path, {
    method, headers,
    body: body ? JSON.stringify(body) : undefined
  });
  if (res.status === 401) { logout(); return null; }
  if (res.status === 204) return true;
  if (!res.ok) {
    const e = await res.json().catch(() => ({}));
    throw new Error(e.message || e.title || 'Error ' + res.status);
  }
  return res.json();
}
const GET  = p      => http('GET',    p);
const POST = (p, b) => http('POST',   p, b);
const PUT  = (p, b) => http('PUT',    p, b);
const DEL  = p      => http('DELETE', p);

// ─── TOAST ───────────────────────────────────────────────────────────────────
function toast(msg, ok = true) {
  const el = document.createElement('div');
  el.className = 'toast ' + (ok ? 'ok' : 'err');
  el.textContent = (ok ? '✅ ' : '❌ ') + msg;
  document.getElementById('toasts').appendChild(el);
  setTimeout(() => el.remove(), 3200);
}

// ─── MODAL ───────────────────────────────────────────────────────────────────
function openModal(html) {
  document.getElementById('modal-box').innerHTML = html;
  document.getElementById('modal-overlay').style.display = 'flex';
}
function closeModal(e) {
  if (!e || e.target === document.getElementById('modal-overlay'))
    document.getElementById('modal-overlay').style.display = 'none';
}

// ─── AUTH ────────────────────────────────────────────────────────────────────
async function login() {
  const email    = document.getElementById('email').value;
  const password = document.getElementById('password').value;
  const btn      = document.getElementById('btn-login');
  btn.textContent = 'Ingresando...'; btn.disabled = true;
  try {
    const data = await POST('/auth/login', { email, password });
    if (!data) return;
    token = data.token;
    currentUser = { nombre: data.nombre, roles: data.roles };
    localStorage.setItem('vt_token', token);
    localStorage.setItem('vt_user', JSON.stringify(currentUser));
    showApp();
  } catch(e) {
    toast(e.message, false);
  } finally {
    btn.textContent = 'Iniciar sesión'; btn.disabled = false;
  }
}

function logout() {
  token = null; currentUser = null;
  localStorage.removeItem('vt_token');
  localStorage.removeItem('vt_user');
  document.getElementById('app').style.display = 'none';
  document.getElementById('login-screen').style.display = 'flex';
}

function showApp() {
  document.getElementById('login-screen').style.display = 'none';
  document.getElementById('app').style.display = 'flex';
  document.getElementById('user-name').textContent = currentUser.nombre;
  go('dashboard');
}

// ─── NAVEGACIÓN ──────────────────────────────────────────────────────────────
const titles = { dashboard:'Dashboard', visitas:'Visitas', visitantes:'Visitantes', empleados:'Empleados', areas:'Áreas' };

async function go(page) {
  document.querySelectorAll('#sidebar nav a').forEach(a => a.classList.remove('active'));
  const link = [...document.querySelectorAll('#sidebar nav a')].find(a => a.textContent.toLowerCase().includes(page));
  if (link) link.classList.add('active');
  document.getElementById('page-title').textContent = titles[page] || page;
  const el = document.getElementById('content');
  el.innerHTML = '<div class="empty">Cargando...</div>';
  try {
    if (page === 'dashboard')  await pageDashboard(el);
    if (page === 'visitas')    await pageVisitas(el);
    if (page === 'visitantes') await pageVisitantes(el);
    if (page === 'empleados')  await pageEmpleados(el);
    if (page === 'areas')      await pageAreas(el);
  } catch(e) {
    el.innerHTML = `<div class="empty">❌ ${e.message}</div>`;
  }
}

// ─── DASHBOARD ───────────────────────────────────────────────────────────────
async function pageDashboard(el) {
  const [visitas, visitantes, empleados] = await Promise.all([GET('/visitas'), GET('/visitantes'), GET('/empleados')]);
  const activas = visitas.filter(v => v.estado === 'EnCurso').length;
  const hoy = new Date().toDateString();
  const hoyN = visitas.filter(v => new Date(v.fechaEntrada).toDateString() === hoy).length;

  el.innerHTML = `
    <div class="stats">
      <div class="stat"><div class="stat-icon">🏃</div><div><div class="stat-val">${activas}</div><div class="stat-lbl">Visitas activas</div></div></div>
      <div class="stat"><div class="stat-icon">📅</div><div><div class="stat-val">${hoyN}</div><div class="stat-lbl">Visitas hoy</div></div></div>
      <div class="stat"><div class="stat-icon">👥</div><div><div class="stat-val">${visitantes.length}</div><div class="stat-lbl">Visitantes</div></div></div>
      <div class="stat"><div class="stat-icon">🧑‍💼</div><div><div class="stat-val">${empleados.length}</div><div class="stat-lbl">Empleados</div></div></div>
    </div>
    <div class="card">
      <div class="card-head"><h3>🏃 Visitas en curso</h3></div>
      <div class="tbl-wrap">${renderTablaVisitas(visitas.filter(v => v.estado === 'EnCurso'), false)}</div>
    </div>`;
}

// ─── VISITAS ─────────────────────────────────────────────────────────────────
async function pageVisitas(el) {
  const visitas = await GET('/visitas');
  el.innerHTML = `
    <div class="card">
      <div class="card-head">
        <h3>Visitas</h3>
        <button class="btn btn-blue" onclick="modalNuevaVisita()">➕ Nueva visita</button>
      </div>
      <div class="card-body">
        <div class="toolbar">
          <input id="buscar-visitas" placeholder="🔍 Buscar..." oninput="filtrar('tbl-visitas', this.value)">
          <select onchange="filtrarEstado(this.value)" style="padding:.45rem .75rem;border:1px solid var(--border);border-radius:var(--radius);font-size:.875rem">
            <option value="">Todos</option>
            <option value="EnCurso">En curso</option>
            <option value="Finalizada">Finalizadas</option>
            <option value="Cancelada">Canceladas</option>
          </select>
        </div>
      </div>
      <div class="tbl-wrap">${renderTablaVisitas(visitas, true)}</div>
    </div>`;
  // guardar para filtrado
  el.dataset.data = JSON.stringify(visitas);
}

function renderTablaVisitas(list, acciones) {
    if (!list.length) return '<div class="empty">No hay visitas</div>';
    const rows = list.map(v => `
    <tr>
      <td>${v.id}</td>
      <td><strong>${v.visitanteNombre}</strong><br><small style="color:var(--muted)">${v.visitanteDocumento}</small></td>
      <td>${v.empleadoNombre}</td>
      <td>${v.areaNombre}</td>
      <td>${fdt(v.fechaEntrada)}</td>
      <td>${v.fechaSalida ? fdt(v.fechaSalida) : '—'}</td>
      <td>${badge(v.estado)}</td>
      <td>${v.motivo}</td>
      ${acciones ? `<td>
        ${v.estado === 'EnCurso' ? `<button class="btn btn-green" onclick="registrarSalida(${v.id})">🚪 Salida</button>` : ''}
        ${v.estado !== 'Finalizada' && v.estado !== 'Cancelada' ? `<button class="btn btn-red" onclick="cancelarVisita(${v.id})">❌ Cancelar</button>` : ''}
      </td>` : ''}
    </tr>`).join('');
    return `<table><thead><tr>
    <th>#</th><th>Visitante</th><th>Responsable</th><th>Área</th>
    <th>Entrada</th><th>Salida</th><th>Estado</th><th>Motivo</th>
    ${acciones ? '<th></th>' : ''}
  </tr></thead><tbody id="tbl-visitas">${rows}</tbody></table>`;
}
async function modalNuevaVisita() {
  const [visitantes, empleados, areas] = await Promise.all([GET('/visitantes'), GET('/empleados'), GET('/areas')]);
  openModal(`
    <div class="modal-head"><h3>Nueva visita</h3><button class="btn-x" onclick="closeModal()">✕</button></div>
    <div class="modal-body">
      <div class="fg"><label>Visitante *</label>
        <select id="m-visitante">
          <option value="">Seleccionar...</option>
          ${visitantes.map(v => `<option value="${v.id}">${v.nombre} ${v.apellido} — ${v.documentoIdentidad}</option>`).join('')}
        </select>
      </div>
      <div class="fg"><label>Área *</label>
        <select id="m-area" onchange="cargarEmps(this.value)">
          <option value="">Seleccionar...</option>
          ${areas.map(a => `<option value="${a.id}">${a.nombre}</option>`).join('')}
        </select>
      </div>
      <div class="fg"><label>Empleado responsable *</label>
        <select id="m-empleado"><option value="">Selecciona un área primero</option></select>
      </div>
      <div class="fg"><label>Motivo *</label>
        <input id="m-motivo" placeholder="Ej: Reunión, entrega de documentos...">
      </div>
    </div>
    <div class="modal-foot">
      <button class="btn btn-gray" onclick="closeModal()">Cancelar</button>
      <button class="btn btn-blue" onclick="crearVisita()">✅ Registrar</button>
    </div>`);
  // guardar empleados para filtrado
  window._empleados = empleados;
}

function cargarEmps(areaId) {
  const sel = document.getElementById('m-empleado');
  const list = (window._empleados || []).filter(e => e.areaId == areaId);
  sel.innerHTML = `<option value="">Seleccionar...</option>` +
    list.map(e => `<option value="${e.id}">${e.nombre} ${e.apellido}</option>`).join('');
}

async function crearVisita() {
  const visitanteId           = +document.getElementById('m-visitante').value;
  const areaId                = +document.getElementById('m-area').value;
  const empleadoResponsableId = +document.getElementById('m-empleado').value;
  const motivo                = document.getElementById('m-motivo').value.trim();
  if (!visitanteId || !areaId || !empleadoResponsableId || !motivo)
    return toast('Completa todos los campos', false);
  try {
    await POST('/visitas', { visitanteId, empleadoResponsableId, areaId, motivo });
    toast('Visita registrada');
    closeModal();
    go('visitas');
  } catch(e) { toast(e.message, false); }
}

async function registrarSalida(id) {
  if (!confirm('¿Registrar salida?')) return;
  try {
    await POST(`/visitas/${id}/salida`);
    toast('Salida registrada');
    go('visitas');
  } catch(e) { toast(e.message, false); }
}

async function cancelarVisita(id) {
    if (!confirm('¿Cancelar esta visita?')) return;
    try {
        await PUT(`/visitas/${id}/cancelar`);
        toast('Visita cancelada');
        go('visitas');
    } catch (e) { toast(e.message, false); }
}

function filtrarEstado(estado) {
  const rows = document.querySelectorAll('#tbl-visitas tr');
  rows.forEach(r => {
    if (!estado) { r.style.display = ''; return; }
    const label = estado === 'EnCurso' ? 'En curso' : estado;
    r.style.display = r.textContent.includes(label) ? '' : 'none';
  });
}

// ─── VISITANTES ───────────────────────────────────────────────────────────────
async function pageVisitantes(el) {
  const list = await GET('/visitantes');
  el.innerHTML = `
    <div class="card">
      <div class="card-head"><h3>Visitantes</h3>
        <button class="btn btn-blue" onclick="modalVisitante()">➕ Nuevo</button>
      </div>
      <div class="card-body">
        <div class="toolbar"><input placeholder="🔍 Buscar..." oninput="filtrar('tbl-visitantes', this.value)"></div>
      </div>
      <div class="tbl-wrap">
        <table><thead><tr>
          <th>#</th><th>Nombre</th><th>Documento</th><th>Email</th><th>Teléfono</th><th>Empresa</th><th></th>
        </tr></thead>
        <tbody id="tbl-visitantes">
          ${list.map(v => `<tr>
            <td>${v.id}</td>
            <td><strong>${v.nombre} ${v.apellido}</strong></td>
            <td>${v.documentoIdentidad}</td>
            <td>${v.email || '—'}</td>
            <td>${v.telefono || '—'}</td>
            <td>${v.empresa || '—'}</td>
            <td>
              <button class="btn btn-gray" onclick="modalVisitante(${v.id})">✏️</button>
              <button class="btn btn-red"  onclick="eliminar('/visitantes/${v.id}','visitantes')">🗑</button>
            </td>
          </tr>`).join('')}
        </tbody></table>
        ${!list.length ? '<div class="empty">No hay visitantes</div>' : ''}
      </div>
    </div>`;
}

async function modalVisitante(id) {
  let v = {};
  if (id) v = await GET('/visitantes/' + id);
  openModal(`
    <div class="modal-head"><h3>${id ? 'Editar' : 'Nuevo'} visitante</h3><button class="btn-x" onclick="closeModal()">✕</button></div>
    <div class="modal-body">
      <div class="row2">
        <div class="fg"><label>Nombre *</label><input id="v-nombre" value="${v.nombre||''}"></div>
        <div class="fg"><label>Apellido *</label><input id="v-apellido" value="${v.apellido||''}"></div>
      </div>
      <div class="fg"><label>Documento *</label><input id="v-doc" value="${v.documentoIdentidad||''}"></div>
      <div class="row2">
        <div class="fg"><label>Email</label><input id="v-email" type="email" value="${v.email||''}"></div>
        <div class="fg"><label>Teléfono</label><input id="v-tel" value="${v.telefono||''}"></div>
      </div>
      <div class="fg"><label>Empresa</label><input id="v-empresa" value="${v.empresa||''}"></div>
    </div>
    <div class="modal-foot">
      <button class="btn btn-gray" onclick="closeModal()">Cancelar</button>
      <button class="btn btn-blue" onclick="guardarVisitante(${id||0})">Guardar</button>
    </div>`);
}

async function guardarVisitante(id) {
  const dto = {
    nombre: document.getElementById('v-nombre').value.trim(),
    apellido: document.getElementById('v-apellido').value.trim(),
    documentoIdentidad: document.getElementById('v-doc').value.trim(),
    email: document.getElementById('v-email').value.trim() || null,
    telefono: document.getElementById('v-tel').value.trim() || null,
    empresa: document.getElementById('v-empresa').value.trim() || null,
  };
  if (!dto.nombre || !dto.apellido || !dto.documentoIdentidad)
    return toast('Nombre, apellido y documento son requeridos', false);
  try {
    if (id) await PUT('/visitantes/' + id, dto);
    else    await POST('/visitantes', dto);
    toast(id ? 'Visitante actualizado' : 'Visitante registrado');
    closeModal(); go('visitantes');
  } catch(e) { toast(e.message, false); }
}

// ─── EMPLEADOS ────────────────────────────────────────────────────────────────
async function pageEmpleados(el) {
  const [list, areas] = await Promise.all([GET('/empleados'), GET('/areas')]);
  window._areas = areas;
  el.innerHTML = `
    <div class="card">
      <div class="card-head"><h3>Empleados</h3>
        <button class="btn btn-blue" onclick="modalEmpleado()">➕ Nuevo</button>
      </div>
      <div class="card-body">
        <div class="toolbar"><input placeholder="🔍 Buscar..." oninput="filtrar('tbl-empleados', this.value)"></div>
      </div>
      <div class="tbl-wrap">
        <table><thead><tr>
          <th>#</th><th>Nombre</th><th>Email</th><th>Puesto</th><th>Área</th><th></th>
        </tr></thead>
        <tbody id="tbl-empleados">
          ${list.map(e => `<tr>
            <td>${e.id}</td>
            <td><strong>${e.nombre} ${e.apellido}</strong></td>
            <td>${e.email}</td>
            <td>${e.puesto || '—'}</td>
            <td>${e.areaNombre}</td>
            <td>
              <button class="btn btn-gray" onclick="modalEmpleado(${e.id})">✏️</button>
              <button class="btn btn-red"  onclick="eliminar('/empleados/${e.id}','empleados')">🗑</button>
            </td>
          </tr>`).join('')}
        </tbody></table>
        ${!list.length ? '<div class="empty">No hay empleados</div>' : ''}
      </div>
    </div>`;
}

async function modalEmpleado(id) {
  let e = {};
  if (id) e = await GET('/empleados/' + id);
  const areas = window._areas || await GET('/areas');
  openModal(`
    <div class="modal-head"><h3>${id ? 'Editar' : 'Nuevo'} empleado</h3><button class="btn-x" onclick="closeModal()">✕</button></div>
    <div class="modal-body">
      <div class="row2">
        <div class="fg"><label>Nombre *</label><input id="e-nombre" value="${e.nombre||''}"></div>
        <div class="fg"><label>Apellido *</label><input id="e-apellido" value="${e.apellido||''}"></div>
      </div>
      <div class="fg"><label>Email *</label><input id="e-email" type="email" value="${e.email||''}"></div>
      <div class="row2">
        <div class="fg"><label>Puesto</label><input id="e-puesto" value="${e.puesto||''}"></div>
        <div class="fg"><label>Área *</label>
          <select id="e-area">
            <option value="">Seleccionar...</option>
            ${areas.map(a => `<option value="${a.id}" ${a.id==e.areaId?'selected':''}>${a.nombre}</option>`).join('')}
          </select>
        </div>
      </div>
    </div>
    <div class="modal-foot">
      <button class="btn btn-gray" onclick="closeModal()">Cancelar</button>
      <button class="btn btn-blue" onclick="guardarEmpleado(${id||0})">Guardar</button>
    </div>`);
}

async function guardarEmpleado(id) {
  const dto = {
    nombre: document.getElementById('e-nombre').value.trim(),
    apellido: document.getElementById('e-apellido').value.trim(),
    email: document.getElementById('e-email').value.trim(),
    puesto: document.getElementById('e-puesto').value.trim() || null,
    areaId: +document.getElementById('e-area').value,
  };
  if (!dto.nombre || !dto.apellido || !dto.email || !dto.areaId)
    return toast('Nombre, apellido, email y área son requeridos', false);
  try {
    if (id) await PUT('/empleados/' + id, dto);
    else    await POST('/empleados', dto);
    toast(id ? 'Empleado actualizado' : 'Empleado registrado');
    closeModal(); go('empleados');
  } catch(e) { toast(e.message, false); }
}

// ─── ÁREAS ────────────────────────────────────────────────────────────────────
async function pageAreas(el) {
  const list = await GET('/areas');
  el.innerHTML = `
    <div class="card">
      <div class="card-head"><h3>Áreas</h3>
        <button class="btn btn-blue" onclick="modalArea()">➕ Nueva</button>
      </div>
      <div class="tbl-wrap">
        <table><thead><tr><th>#</th><th>Nombre</th><th>Descripción</th><th></th></tr></thead>
        <tbody>
          ${list.map(a => `<tr>
            <td>${a.id}</td>
            <td><strong>${a.nombre}</strong></td>
            <td>${a.descripcion || '—'}</td>
            <td>
              <button class="btn btn-gray" onclick="modalArea(${a.id})">✏️</button>
              <button class="btn btn-red"  onclick="eliminar('/areas/${a.id}','areas')">🗑</button>
            </td>
          </tr>`).join('')}
        </tbody></table>
        ${!list.length ? '<div class="empty">No hay áreas</div>' : ''}
      </div>
    </div>`;
}

async function modalArea(id) {
  let a = {};
  if (id) a = await GET('/areas/' + id);
  openModal(`
    <div class="modal-head"><h3>${id ? 'Editar' : 'Nueva'} área</h3><button class="btn-x" onclick="closeModal()">✕</button></div>
    <div class="modal-body">
      <div class="fg"><label>Nombre *</label><input id="a-nombre" value="${a.nombre||''}"></div>
      <div class="fg"><label>Descripción</label><input id="a-desc" value="${a.descripcion||''}"></div>
    </div>
    <div class="modal-foot">
      <button class="btn btn-gray" onclick="closeModal()">Cancelar</button>
      <button class="btn btn-blue" onclick="guardarArea(${id||0})">Guardar</button>
    </div>`);
}

async function guardarArea(id) {
  const dto = {
    nombre: document.getElementById('a-nombre').value.trim(),
    descripcion: document.getElementById('a-desc').value.trim() || null,
  };
  if (!dto.nombre) return toast('El nombre es requerido', false);
  try {
    if (id) await PUT('/areas/' + id, dto);
    else    await POST('/areas', dto);
    toast(id ? 'Área actualizada' : 'Área creada');
    closeModal(); go('areas');
  } catch(e) { toast(e.message, false); }
}

// ─── HELPERS ─────────────────────────────────────────────────────────────────
async function eliminar(path, page) {
  if (!confirm('¿Confirmar eliminación?')) return;
  try {
    await DEL(path);
    toast('Eliminado correctamente');
    go(page);
  } catch(e) { toast(e.message, false); }
}

function filtrar(tbodyId, term) {
  const t = term.toLowerCase();
  document.querySelectorAll('#' + tbodyId + ' tr').forEach(r => {
    r.style.display = r.textContent.toLowerCase().includes(t) ? '' : 'none';
  });
}

function badge(estado) {
  const map = { EnCurso: ['badge-green','En curso'], Finalizada: ['badge-gray','Finalizada'], Cancelada: ['badge-red','Cancelada'] };
  const [cls, lbl] = map[estado] || ['badge-yellow', estado];
  return `<span class="badge ${cls}">${lbl}</span>`;
}

function fdt(iso) {
  if (!iso) return '—';
  const d = new Date(iso);
  return d.toLocaleDateString('es-DO') + ' ' + d.toLocaleTimeString('es-DO', { hour:'2-digit', minute:'2-digit' });
}

// ─── INICIO ───────────────────────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
  token = localStorage.getItem('vt_token');
  const u = localStorage.getItem('vt_user');
  if (token && u) {
    currentUser = JSON.parse(u);
    showApp();
  }
  document.getElementById('password').addEventListener('keydown', e => {
    if (e.key === 'Enter') login();
  });
});
