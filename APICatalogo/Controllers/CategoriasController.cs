﻿using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return _context.Categorias.Include(p=> p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        try
        {
            return _context.Categorias.AsNoTracking().ToList();
        }
        catch (Exception)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tratar a sua solicitação");
        }
    }

    [HttpGet("{id:int}", Name = "ObertCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        try
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
            if (categoria is null)
                return NotFound("Categoria não encontrada");
            return Ok(categoria);
        }
        catch (Exception)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tratar a sua solicitação");
        }
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();
        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObertCategoria", new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("id:int")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest();

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(categoria);
    }
    [HttpDelete("id:int")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
        if (categoria is null)
            return NotFound("Categoria não localizada");

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return Ok(categoria);
    }
}