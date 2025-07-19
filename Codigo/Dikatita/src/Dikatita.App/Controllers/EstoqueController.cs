using AutoMapper;
using Dikatita.App.Models;
using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dikatita.App.Controllers;

[Authorize(Roles = "Admin,Supervisor")]
public class EstoqueController : BaseController
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMovEstoqueRepository _movEstoqueRepository;
    private readonly IMapper _mapper;

    public EstoqueController(
        IProdutoRepository produtoRepository,
        IMovEstoqueRepository movEstoqueRepository,
        IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _movEstoqueRepository = movEstoqueRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _produtoRepository.ObterTodos();
        return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(produtos));
    }

    public async Task<IActionResult> AjusteEstoque()
    {
        var viewModel = new MovEstoqueViewModel
        {
            Produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos())
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AjusteEstoque(MovEstoqueViewModel movEstoqueViewModel)
    {
        if (!ModelState.IsValid)
        {
            movEstoqueViewModel.Produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
            return View(movEstoqueViewModel);
        }

        try
        {
            // Atualizar o estoque do produto
            await _produtoRepository.AtualizarEstoque(
                movEstoqueViewModel.ProdutoId, 
                movEstoqueViewModel.Quantidade,
                movEstoqueViewModel.TipoMovimentacao);

            // Registrar a movimentação
            var movEstoque = _mapper.Map<MovEstoque>(movEstoqueViewModel);
            movEstoque.DataMovimentacao = DateTime.Now;

            await _movEstoqueRepository.Adicionar(movEstoque);

            TempData["Sucesso"] = "Movimentação de estoque realizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            movEstoqueViewModel.Produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
            return View(movEstoqueViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao processar a movimentação: " + ex.Message);
            movEstoqueViewModel.Produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
            return View(movEstoqueViewModel);
        }
    }

    public async Task<IActionResult> Historico(Guid id)
    {
        var produto = await _produtoRepository.ObterPorId(id);
        if (produto == null) return NotFound();

        var movimentacoes = await _movEstoqueRepository.ObterMovimentacoesPorProduto(id);

        var viewModel = new ProdutoViewModel
        {
            Id = produto.Id,
            Nome = produto.Nome,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            Movimentacoes = _mapper.Map<IEnumerable<MovEstoqueViewModel>>(movimentacoes)
        };

        return View(viewModel);
    }
} 