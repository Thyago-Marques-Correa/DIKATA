using AutoMapper;
using Dikatita.App.ViewModels;
using Dikatita.Business.Enums;
using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dikatita.App.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("pedido/[action]")]
public class PedidoController(IMapper mapper, IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository) : BaseController
{
    [HttpGet("listagem-pedidos")]
    public async Task<IActionResult> Index()
    {
        var pedidos = mapper.Map<IEnumerable<PedidoViewModel>>(await pedidoRepository.ObterTodos());
        return View(pedidos);
    }
    
    [HttpGet("detalhes-pedido/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var pedidos = mapper.Map<PedidoViewModel>(await pedidoRepository.ObterPedidoComItensPeloId(id));
        if (pedidos == null) return NotFound();
        return View(pedidos);
    }

    [HttpGet("editar-pedido/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var pedido = mapper.Map<EdicaoPedidoViewModel>(await pedidoRepository.ObterPedidoComItensPeloId(id));
        if (pedido == null) return NotFound();
        return View(pedido);
    }

    [HttpPost("editar-pedido/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, EdicaoPedidoViewModel edicaoPedidoViewModel)
    {
        if (id != edicaoPedidoViewModel.Id) return NotFound();

        if (!ModelState.IsValid) return View(edicaoPedidoViewModel);

        var pedidoAtualizacao = await pedidoRepository.ObterPedidoComItensPeloId(id);
        pedidoAtualizacao.Status = edicaoPedidoViewModel.Status;

        if (edicaoPedidoViewModel.Status == StatusPedido.Concluído)
        {
            var produtoIds = pedidoAtualizacao.Itens.Select(i => i.ProdutoId).Distinct().ToList();
            var produtos = await produtoRepository.ObterPorIds(produtoIds);
            var dictProdutos = produtos.ToDictionary(p => p.Id);

            foreach (var item in pedidoAtualizacao.Itens)
            {
                if (dictProdutos.TryGetValue(item.ProdutoId, out var produto))
                {
                    produto.QuantidadeEstoque -= item.Quantidade;
                    if (produto.QuantidadeEstoque < 0) produto.QuantidadeEstoque = 0;
                }
            }

            foreach (var produto in dictProdutos.Values)
            {
                await produtoRepository.Atualizar(produto);
            }
        }

        var pedido = mapper.Map<Pedido>(pedidoAtualizacao);
        await pedidoRepository.Atualizar(pedido);

        return RedirectToAction(nameof(Index));
    }
}