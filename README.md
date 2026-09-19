# 📄 Sistema de Cadastro de Currículos

Aplicação ASP.NET Core MVC para cadastro, edição, exclusão e exibição formatada de currículos, desenvolvida como atividade acadêmica da disciplina de Linguagem de Programação.

## 🚀 Tecnologias

- ASP.NET Core 3.1 (MVC)
- PostgreSQL (via Npgsql)
- HTML5 / CSS3
- Razor Views

## ✨ Funcionalidades

- Listagem dos currículos cadastrados (CPF e Nome)
- Inclusão de novo currículo, contendo:
  - Dados pessoais (nome, CPF, endereço, telefone, e-mail, pretensão salarial, cargo pretendido)
  - Até 5 registros de formação acadêmica (mínimo de 1 obrigatório)
  - Até 3 registros de experiência profissional
  - Até 3 idiomas
- Alteração de currículo existente
- Exclusão de currículo (com remoção em cascata dos dados relacionados)
- Exibição do currículo em página formatada com CSS, com aparência profissional

## 🗂️ Estrutura do banco de dados

Modelo relacional dividido em 4 tabelas:

- `curriculo` — dados pessoais (tabela principal)
- `formacao` — formação acadêmica (N:1 com `curriculo`)
- `experiencia` — experiência profissional (N:1 com `curriculo`)
- `idioma` — idiomas (N:1 com `curriculo`)

A exclusão de um currículo remove automaticamente (`ON DELETE CASCADE`) todos os registros de formação, experiência e idioma associados a ele.

## ▶️ Como rodar localmente

### Pré-requisitos
- .NET Core 3.1 SDK
- PostgreSQL instalado e rodando localmente

### Passos

1. Clone o repositório

2. Crie um banco de dados no PostgreSQL (ex: `controlecurriculo`)

3. Execute os scripts SQL de criação das tabelas (disponíveis na seção abaixo) no seu banco

4. Configure sua string de conexão criando o arquivo `appsettings.Development.json` na raiz do projeto (esse arquivo não é versionado, por segurança):
```json
   {
     "ConnectionStrings": {
       "PostgresConnection": "Host=localhost;Port=5432;Database=controlecurriculo;Username=postgres;Password=SUA_SENHA"
     }
   }
```

5. Rode o projeto

## 🧱 Scripts SQL

```sql
CREATE TABLE curriculo (
    id SERIAL PRIMARY KEY,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL,
    endereco VARCHAR(150) NULL,
    telefone VARCHAR(20) NULL,
    email VARCHAR(100) NULL,
    pretensao_salarial DECIMAL(10,2) NULL,
    cargo_pretendido VARCHAR(100) NULL
);

CREATE TABLE formacao (
    id SERIAL PRIMARY KEY,
    curriculo_id INT NOT NULL REFERENCES curriculo(id) ON DELETE CASCADE,
    curso VARCHAR(100) NOT NULL,
    instituicao VARCHAR(100) NOT NULL,
    ano_conclusao INT NULL
);

CREATE TABLE experiencia (
    id SERIAL PRIMARY KEY,
    curriculo_id INT NOT NULL REFERENCES curriculo(id) ON DELETE CASCADE,
    empresa VARCHAR(100) NULL,
    cargo VARCHAR(100) NULL,
    periodo VARCHAR(50) NULL,
    descricao VARCHAR(300) NULL
);

CREATE TABLE idioma (
    id SERIAL PRIMARY KEY,
    curriculo_id INT NOT NULL REFERENCES curriculo(id) ON DELETE CASCADE,
    nome VARCHAR(50) NULL,
    nivel VARCHAR(30) NULL
);
```


## 👤 Autor

**Raphael Mendonça Riquetto**

[LinkedIn](https://linkedin.com/in/raphaelriquetto) · [GitHub](https://github.com/Raphareuuu)
