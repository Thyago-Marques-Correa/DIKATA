using AutoMapper;
using Dikatita.App.Extensions;
using Dikatita.App.Models;
using Dikatita.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dikatita.App.Controllers;

[Route("carrinho/[action]")]
public class CarrinhoController(IProdutoRepository produtoRepository, IHttpContextAccessor contextAccessor, IMapper mapper) : Controller
{
    private readonly string CarrinhoSessionKey = "CarrinhoSession";

    public IActionResult Index()
    {
        var carrinho = contextAccessor.HttpContext!.Session.GetObject<CarrinhoProdutoViewModel>(CarrinhoSessionKey) 
                       ?? new CarrinhoProdutoViewModel();

        decimal valorTotal = 0;
        foreach (var produto in carrinho.Produtos)
        {
            valorTotal += produto.QuantidadeCarrinho * produto.Valor;
        }

        ViewBag.ValorTotal = valorTotal;

        return View(carrinho);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAoCarrinho(Guid id, int quantidade)
    {
        var produtoViewModel = mapper.Map<ProdutoViewModel>(await produtoRepository.ObterPorId(id));
        if (produtoViewModel == null) return NotFound();

        var carrinho = contextAccessor.HttpContext!.Session.GetObject<CarrinhoProdutoViewModel>(CarrinhoSessionKey) 
                       ?? new CarrinhoProdutoViewModel();

        var produtoExistente = carrinho.Produtos.FirstOrDefault(x => x.Id == id);

        if (produtoExistente != null)
        {
            produtoExistente.QuantidadeCarrinho += quantidade;
        }
        else
        {
            produtoViewModel.QuantidadeCarrinho = quantidade;
            carrinho.Produtos.Add(produtoViewModel);
        }

        contextAccessor.HttpContext.Session.SetObject(CarrinhoSessionKey, carrinho);
        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    public IActionResult RemoverDoCarrinho(Guid id)
    {
        var carrinho = contextAccessor.HttpContext!.Session.GetObject<CarrinhoProdutoViewModel>(CarrinhoSessionKey);

        var produtoExistente = carrinho.Produtos.FirstOrDefault(x => x.Id == id);
        if (produtoExistente == null) return RedirectToAction("Index");
        
        carrinho.Produtos.Remove(produtoExistente);
        contextAccessor.HttpContext.Session.SetObject(CarrinhoSessionKey, carrinho);

        return RedirectToAction("Index");
    }
}