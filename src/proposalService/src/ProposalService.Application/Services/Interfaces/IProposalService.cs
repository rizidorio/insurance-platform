using Insurence.Platform.Common.Wrappers;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.DataTransferObjects.Responses;

namespace ProposalService.Application.Services.Interfaces;

/// <summary>
/// Define o contrato para gerenciamento de propostas, incluindo operações para criar, recuperar, aprovar e rejeitar propostas.
/// </summary>
/// <remarks>
/// Implementações desta interface devem fornecer métodos assíncronos para manipulação de ações relacionadas a propostas.
/// Todos os métodos suportam cancelamento via <see cref="CancellationToken"/>.
/// O serviço é normalmente utilizado em fluxos de negócio onde propostas precisam ser rastreadas, filtradas e atualizadas conforme identificadores de cliente ou externos.
/// </remarks>
public interface IProposalService
{
    /// <summary>
    /// Recupera uma lista paginada de propostas que correspondem aos critérios de filtro especificados de forma assíncrona.
    /// </summary>
    /// <param name="request">Objeto contendo os parâmetros de filtro para busca de propostas. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com uma lista paginada de propostas que atendem aos critérios de filtro.
    /// </returns>
    Task<ResponseDefault<PaginatedResponse<ProposalResponse>>> GetByFilterAsync(GetProposalFilterRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera assíncronamente uma lista de propostas associadas ao número de documento do cliente especificado.
    /// </summary>
    /// <param name="request">Objeto contendo o número de documento do cliente e critérios adicionais para filtrar propostas.</param>
    /// <param name="cancellationToken">Token que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com uma lista de propostas que correspondem ao número de documento do cliente especificado. Se nenhuma proposta for encontrada, a lista estará vazia.
    /// </returns>
    Task<ResponseDefault<IList<ProposalResponse>>> GetByClientDocumentNumberAsync(GetProposalsByClientDocumentNumberRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera uma proposta pelo seu identificador externo de forma assíncrona.
    /// </summary>
    /// <param name="request">Objeto contendo o identificador externo e parâmetros adicionais necessários para localizar a proposta.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com os dados da proposta, se encontrada; caso contrário, a resposta indica o resultado da busca.
    /// </returns>
    Task<ResponseDefault<ProposalResponse>> GetByExternalIdAsync(GetProposalByIdRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Cria uma nova proposta de forma assíncrona utilizando os dados especificados na requisição.
    /// </summary>
    /// <param name="request">Detalhes da proposta a ser criada. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser utilizado para cancelar a operação.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com os detalhes da proposta criada.
    /// </returns>
    Task<ResponseDefault<ProposalResponse>> CreateAsync(CreateProposalRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Inicia uma operação assíncrona para aprovar uma proposta utilizando os parâmetros especificados.
    /// </summary>
    /// <param name="request">Objeto contendo os detalhes da proposta a ser aprovada. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token que pode ser utilizado para cancelar a operação de aprovação.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com o resultado da aprovação da proposta.
    /// </returns>
    Task<ResponseDefault<ProposalResponse>> ApproveAsync(ApproveProposalRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Rejeita uma proposta de forma assíncrona com base nos parâmetros especificados.
    /// </summary>
    /// <param name="request">Objeto contendo os detalhes da proposta a ser rejeitada. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com o resultado da rejeição, incluindo status ou informações de erro relevantes.
    /// </returns>
    Task<ResponseDefault<ProposalResponse>> RejectAsync(RejectProposalRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Analisa riscos de forma assíncrona com base nos parâmetros especificados na requisição.
    /// </summary>
    /// <param name="request">A requisição de análise de risco contendo dados de entrada e opções de configuração. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém um objeto de resposta com os resultados da análise de risco.</returns>
    Task<ResponseDefault<RiskAnalysisResponse>> GetRiskAnalysisAsync(RiskAnalysisRequest request, CancellationToken cancellationToken);
}
