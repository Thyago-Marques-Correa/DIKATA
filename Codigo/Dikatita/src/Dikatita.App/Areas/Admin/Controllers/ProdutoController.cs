using AutoMapper;
using Dikatita.App.Models;
using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Estudos.App.Web.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dikatita.App.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("produtos/[action]")]
public class ProdutoController(IProdutoRepository produtoRepository, IConfiguration configuration, IMapper mapper) : BaseController
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet("listagem-produtos")]
    public async Task<IActionResult> Index()
    {
        var produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
        return View(produtos);
    }

    [HttpGet("detalhes-produto/{id:guid}")]
    public async Task<IActionResult> Detalhes(Guid id)
    {
        var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        if (produtoViewModel == null) return NotFound();
        return View(produtoViewModel);
    }

    [HttpGet("criar-produto")]
    public ActionResult Criar()
    {
        return View();
    }

    [HttpPost("criar-produto")]
    public async Task<IActionResult> Criar(ProdutoViewModel produtoViewModel)
    {
        var caminho = await FileHelper.UploadArquivo(produtoViewModel.ImagemUpload, _configuration);

        if (!ModelState.IsValid || string.IsNullOrEmpty(caminho))
            return View(produtoViewModel);

        var produto = _mapper.Map<Produto>(produtoViewModel);
        produto.Imagem = caminho;

        await _produtoRepository.Adicionar(produto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar-produto/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id)
    {
        var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        if (produtoViewModel == null) return NotFound();
        return View(produtoViewModel);
    }

    [HttpPost("editar-produto/{id:guid}")]
    public async Task<IActionResult> Editar(Guid id, ProdutoViewModel produtoViewModel)
    {
        if (id != produtoViewModel.Id) return NotFound();

        var produtoAtualizacao = await _produtoRepository.ObterPorId(id);
        produtoViewModel.Imagem = produtoAtualizacao.Imagem;

        ModelState.Remove(nameof(produtoViewModel.ImagemUpload));
        if (!ModelState.IsValid) return View(produtoViewModel);

        if (produtoViewModel.ImagemUpload != null)
        {
            var caminho = await FileHelper.UploadArquivo(produtoViewModel.ImagemUpload, _configuration);
            if (string.IsNullOrEmpty(caminho))
            {
                ModelState.AddModelError(string.Empty, "Erro ao realizar upload da imagem");
                return View(produtoViewModel);
            }
            produtoAtualizacao.Imagem = caminho;
        }

        produtoAtualizacao.Ativo = produtoViewModel.Ativo;
        produtoAtualizacao.Descricao = produtoViewModel.Descricao;
        produtoAtualizacao.Valor = produtoViewModel.Valor;
        produtoAtualizacao.Nome = produtoViewModel.Nome;
        produtoAtualizacao.QuantidadeEstoque = produtoViewModel.QuantidadeEstoque;

        var produto = _mapper.Map<Produto>(produtoAtualizacao);
        await _produtoRepository.Atualizar(produto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("atualizar-estoque/{id:guid}")]
    public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(id);
        if (produto == null) return NotFound();

        produto.QuantidadeEstoque = quantidade;
        await _produtoRepository.Atualizar(produto);

        return RedirectToAction(nameof(Index));
    }
}
