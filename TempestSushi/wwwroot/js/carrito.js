// ══ Carrito de compras del Cliente ═══════════════════════════════════
// Guarda SOLO tipo/idItem/cantidad en localStorage. El nombre, precio,
// subtotal e impuesto SIEMPRE se piden en vivo al servidor (endpoint
// /Pedido/CalcularLinea) para cumplir con "toda la información debe
// provenir de la BD" — nunca se pinta un precio guardado en el navegador.

const TS_CARRITO_KEY = 'ts_carrito_items';

function tsCarritoObtener() {
    try {
        const raw = localStorage.getItem(TS_CARRITO_KEY);
        const items = raw ? JSON.parse(raw) : [];
        return Array.isArray(items) ? items : [];
    } catch (e) {
        return [];
    }
}

function tsCarritoGuardar(items) {
    localStorage.setItem(TS_CARRITO_KEY, JSON.stringify(items));
    tsCarritoActualizarBadge();
    window.dispatchEvent(new CustomEvent('ts:carrito-actualizado', { detail: { items } }));
}

// Agrega una unidad (o "cantidad") de un producto/combo. Si ya existe, suma cantidad.
function tsCarritoAgregar(tipo, idItem, cantidad) {
    cantidad = cantidad || 1;
    const items = tsCarritoObtener();
    const existente = items.find(i => i.tipo === tipo && i.idItem === idItem);
    if (existente) {
        existente.cantidad += cantidad;
    } else {
        items.push({ tipo: tipo, idItem: idItem, cantidad: cantidad });
    }
    tsCarritoGuardar(items);
}

function tsCarritoActualizarCantidad(tipo, idItem, cantidad) {
    let items = tsCarritoObtener();
    if (cantidad <= 0) {
        items = items.filter(i => !(i.tipo === tipo && i.idItem === idItem));
    } else {
        const existente = items.find(i => i.tipo === tipo && i.idItem === idItem);
        if (existente) existente.cantidad = cantidad;
    }
    tsCarritoGuardar(items);
}

function tsCarritoQuitar(tipo, idItem) {
    tsCarritoActualizarCantidad(tipo, idItem, 0);
}

function tsCarritoVaciar() {
    tsCarritoGuardar([]);
}

function tsCarritoContarItems() {
    return tsCarritoObtener().reduce((acc, i) => acc + i.cantidad, 0);
}

function tsCarritoActualizarBadge() {
    const badge = document.getElementById('ts-carrito-badge');
    if (!badge) return;
    const cantidad = tsCarritoContarItems();
    badge.textContent = cantidad;
    badge.style.display = cantidad > 0 ? 'inline-flex' : 'none';
}

// Toast simple reutilizable si la página no define el suyo propio
function tsMostrarToastGlobal(mensaje, tipo) {
    let toast = document.getElementById('ts-toast-global');
    if (!toast) {
        toast = document.createElement('div');
        toast.id = 'ts-toast-global';
        toast.style.cssText = 'position:fixed;bottom:24px;right:24px;padding:12px 20px;border-radius:10px;' +
            'font-family:"Rajdhani",sans-serif;font-weight:700;color:#fff;z-index:9999;' +
            'opacity:0;transition:opacity .25s;pointer-events:none;';
        document.body.appendChild(toast);
    }
    toast.style.background = tipo === 'error' ? '#c0392b' : '#1a3a6e';
    toast.textContent = mensaje;
    toast.style.opacity = '1';
    clearTimeout(toast._timeoutId);
    toast._timeoutId = setTimeout(() => { toast.style.opacity = '0'; }, 2500);
}

document.addEventListener('DOMContentLoaded', tsCarritoActualizarBadge);
