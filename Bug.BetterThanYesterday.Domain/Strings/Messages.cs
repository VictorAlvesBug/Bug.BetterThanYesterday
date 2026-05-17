

namespace Bug.BetterThanYesterday.Domain.Strings;

public static class Messages
{
    public const string GenericError = "Ops! Ocorreu um erro";

    #region Admin Settings
    public const string EnterDaysAmountToMoveInTime = "Defina uma quantidade de dias para viajar no tempo";
    #endregion

    #region Check-In Messages
    public const string CheckInNotFound = "Check-in não encontrado";
    public const string CheckInSuccessfullyFound = "Check-in encontrado com sucesso";
    public const string CheckInsSuccessfullyFound = "Check-ins encontrados com sucesso";
    public const string CheckInSuccessfullyRegistered = "Check-in cadastrado com sucesso";
    public const string EnterCheckInDate = "Informe a data do check-in";
    public const string EnterCheckInId = "Informe o ID do Check-in";
    public const string EnterCheckInIndex = "Informe o índice do check-in";
    public const string EnterCheckInTitle = "Informe o título do check-in";
    public const string EnterCheckInCreateDate = "Informe a data de criação do check-in";
    public const string CheckInAlreadyExists = "Check-in já existe";
    public const string PlanUserDateSuccessfullyFound = "Plano, usuário e data encontrados com sucesso";
    public const string PlanUserSuccessfullyFound = "Plano e usuário encontrados com sucesso";
    public const string OnlyRunningPlansCanReceiveNewCheckIns = "Apenas planos em execução podem receber novos check-ins";
    public const string OnlyActiveMembersCanMakeCheckIns = "Apenas membros ativos podem fazer check-ins";
    public const string UserHasReachedTheMaximumNumberOfCheckInsForTheDay = "Usuário atingiu o número máximo de check-ins para o dia";
    public const string EnterCheckInStatus = "Informe o status do check-ins";
    public const string EnterCheckInPhotoUrl = "Informe o URL da foto de evidência";
    public const string EnterReviewerId = "Informe o ID do avaliador";
    public const string InvalidReviewStatus = "Status de avaliação inválido";
    public const string InvalidReviewDate = "Data da avaliação inválida";
    public const string ProvidePlanIdInOrderToUseAlsoOtherFilters = "Forneça o ID do plano para utilizar os demais filtros";
    #endregion

    #region Habit Messages
    public const string EnterHabitCreationDate = "Informe a data de criação do hábito";
    public const string EnterHabitId = "Informe o ID do hábito";
    public const string EnterHabitName = "Informe o nome do hábito";
    public const string HabitCannotBeRemovedAsItHasLinkedPlans = "Hábito não pode ser removido, pois possui planos vinculados";
    public const string HabitNotFound = "Hábito não encontrado";
    public const string HabitSuccessfullyDeleted = "Hábito deletado com sucesso";
    public const string HabitSuccessfullyFound = "Hábito encontrado com sucesso";
    public const string HabitsSuccessfullyFound = "Hábitos encontrados com sucesso";
    public const string HabitSuccessfullyRegistered = "Hábito cadastrado com sucesso";
    public const string HabitSuccessfullyUpdated = "Hábito atualizado com sucesso";
    public const string ThereIsAlreadyAHabitRegisteredWithThatName = "Já existe um hábito cadastrado com esse nome";
    #endregion

    #region Plan Messages
    public const string EnterOwnerId = "Informe o ID do criador do plano";
    public const string EnterPlanEndDate = "Informe a data final do plano";
    public const string EnterPlanId = "Informe o ID do plano";
    public const string EnterPlanCreationDate = "Informe a data de criação do plano";
    public const string EnterPlanStatus = "Informe o status do plano";
    public const string EnterPlanStartDate = "Informe a data inicial do plano";
    public const string PlanHasNoMembers = "Plano não possui membros";
    public const string PlanNotFound = "Plano não encontrado";
    public const string EnterPlanType = "Informe o tipo de plano";
    public const string EnterValidPlanType = "Informe um tipo de plano válido";
    public const string EnterValidDaysOffPerWeek = "Informe uma quantidade de folgas semanas válida";
    public const string EnterValidPenaltyValue = "Informe um valor de penalidade válido";
    public const string PlanStatusSuccessfullyUpdated = "Status do plano atualizado com sucesso";
    public const string PlanSuccessfullyCancelled = "Plano cancelado com sucesso";
    public const string PlanSuccessfullyFound = "Plano encontrado com sucesso";
    public const string PlansSuccessfullyFound = "Planos encontrados com sucesso";
    public const string PlanSuccessfullyRegistered = "Plano cadastrado com sucesso";
    #endregion

    #region Plan Member Messages
    public const string EnterPlanMemberId = "Informe o ID do membro no plano";
    public const string EnterPlanMemberJoinedDate = "Informe a data de inclusão do usuário no plano";
    public const string OnlyNotStartedPlansCanReceiveNewMembers = "Apenas planos não iniciados podem receber novos membros";
    public const string MemberAlreadyAddedToThePlan = "Membro já adicionado ao plano";
    public const string MemberSuccessfullyAddedToThePlan = "Membro adicionado ao plano com sucesso";
    public const string MemberSuccessfullyReaddedToThePlan = "Membro readicionado ao plano com sucesso";
    public const string MemberSuccessfullyBlocked = "Membro bloqueado com sucesso";
    public const string MemberSuccessfullyUnblocked = "Membro desbloqueado com sucesso";
    public const string MemberSuccessfullyRemovedFromThePlan = "Membro removido do plano com sucesso";
    public const string PlanMemberNotFound = "Membro não encontrado no plano";
    public const string PlanMemberStatusIdMustBeGreaterThanZero = "O ID do status do membro deve ser maior que zero";
    public const string EnterPlanMemberCreateDate = "Informe a data de criação do membro do plano";
    public const string PlanMemberSuccessfullyFound = "Membro do plano encontrado com sucesso";
    public const string MemberAlreadyBlockedInThisPlan = "Este membro já está bloqueado neste plano";
    public const string MemberCannotBeRemovedFromThePlanAsHeIsBlocked = "Este membro não pode ser removido do plano, pois está bloqueado";
    public const string MemberCanOnlyBeBlockedInRunningPlans = "Membros só podem ser bloqueados em planos em andamento";
    public const string MemberCanOnlyBeRemovedFromNotStartedOrRunningPlans = "Membros só podem ser removidos de planos não iniciados ou em andamento";
    public const string MemberCanOnlyBeUnblockedInRunningPlans = "Membros só podem ser desbloqueados em planos em andamento";
    public const string MemberHasAlreadyBeenOnThisPlanBefore = "Membro já esteve neste plano antes";
    public const string MemberIsAlreadyBlockedInThePlan = "Membro já está bloqueado neste plano";
    public const string MemberIsAlreadyActiveInThePlan = "Membro já está ativo neste plano";
    public const string MemberIsBlockedInThePlan = "Membro está bloqueado neste plano";
    public const string UserIsNotInThePlan = "Usuário não está no plano";
    public const string UserHasNoPlans = "Usuário não possui planos";
    #endregion

    #region User Messages
    public const string UserEmailAlreadyRegistered = "E-mail já cadastrado para outro usuário";
    public const string EnterUserEmail = "Informe seu e-mail";
    public const string EnterUserId = "Informe o ID de usuário";
    public const string EnterUserName = "Informe seu nome";
    public const string EnterUserNameOrEmail = "Informe seu nome ou e-mail";
    public const string EnterUserNickname = "Informe um nickname (apelido)";
    public const string EnterUserPhoneNumber = "Informe seu número de celular";
    public const string EnterValidUserPhoneNumber = "Informe um número de celular válido";
    public const string EnterUserPixKey = "Informe sua chave PIX";
    public const string EnterValidUserPixKey = "Informe uma chave PIX válida";
    public const string EnterValidCpfTaxIdentification = "Informe um CPF válido";
    public const string EnterValidCnpjTaxIdentification = "Informe um CNPJ válido";
    public const string EnterValidTaxIdentification = "Informe um número de documento válido";
    public const string EnterValidEVP = "Informe um endereço virtual de pagamento válido para a chave PIX";
    public const string EnterUserPixKeyType = "Informe um tipo válido para a chave PIX";
    public const string EnterUserCreationDate = "Informe a data de criação do usuário";
    public const string EnterValidUserEmail = "Informe um e-mail válido para o usuário";
    public const string UserNotFound = "Usuário não encontrado";
    public const string UserSuccessfullyDeleted = "Usuário deletado com sucesso";
    public const string UserSuccessfullyFound = "Usuário encontrado com sucesso";
    public const string UsersSuccessfullyFound = "Usuários encontrados com sucesso";
    public const string UserSuccessfullyRegistered = "Usuário cadastrado com sucesso";
    public const string UserSuccessfullyUpdated = "Usuário atualizado com sucesso";
    #endregion

    #region Other Messages
    public const string ChangeNotMappedToCurrentStatus = "Alteração não mapeada para status atual";
    public const string EndDateMustBeLaterThanStartDate = "Data final deve ser posterior à data inicial";
    public const string OnlyValidGuidsCanBeCombined = "Apenas GUIDs válidos podem ser combinados";
    public const string StartDateCannotBeEarlierThanToday = "Data de início não pode ser anterior a hoje";
    public const string EnterPhoto = "Adicione uma foto";
    public const string EnterValidPhoto = "Informe um caminho válido para a imagem";
    #endregion
}