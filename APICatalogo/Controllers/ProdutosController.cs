﻿using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetProdutos()
    {
        var produtos = _context.Produtos.ToList();
        if (produtos is null)
            return NotFound("Produtos não encontrados");
        return produtos;
    }

    [HttpGet("{id:int}", Name = "ObertProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
        if (produto is null)
            return NotFound("Produto não encontrado");
        return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest();
        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObertProduto", new { id = produto.ProdutoId }, produto);   
    }

    [HttpPut("id:int")]
    public ActionResult Put(int id, Produto produto) 
    {
        if (id != produto.ProdutoId)
            return BadRequest();

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }
    [HttpDelete("id:int")]
    public ActionResult Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId==id);
        if (produto is null)
            return NotFound("Produto não localizado");

        _context.Produtos.Remove(produto);
        _context.SaveChanges();
        return Ok(produto);
        
    }
}
