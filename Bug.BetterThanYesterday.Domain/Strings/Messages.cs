
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Strings;

public static class Messages
{
    public const string GenericError = "Ops! Algo saiu do trilho";

    #region Admin Settings
    public const string EnterDaysAmountToMoveInTime = "Diz quantos dias você quer avançar no tempo";
    #endregion

    #region Check-In Messages
    public const string CheckInNotFound = "Esse check-in sumiu do mapa";
    public const string CheckInSuccessfullyFound = "Check-in encontrado";
    public const string CheckInsSuccessfullyFound = "Check-ins encontrados";
    public const string CheckInSuccessfullyRegistered = "Check-in na conta. Sequência protegida!";
    public const string EnterCheckInDate = "Escolhe o dia do check-in";
    public const string EnterCheckInId = "Faltou o ID do check-in";
    public const string EnterCheckInIndex = "Faltou o índice do check-in";
    public const string EnterCheckInTitle = "Dá um título para o seu check-in";
    public const string EnterCheckInCreateDate = "Faltou a data de criação do check-in";
    public const string CheckInAlreadyExists = "Esse dia já tem check-in";
    public const string ChallengeUserDateSuccessfullyFound = "Desafio, jogador e data encontrados";
    public const string ChallengeUserSuccessfullyFound = "Desafio e jogador encontrados";
    public const string OnlyRunningChallengesCanReceiveNewCheckIns = "Só dá para marcar check-in com o desafio em andamento";
    public const string OnlyActiveMembersCanMakeCheckIns = "Só quem está na partida pode marcar check-in";
    public const string CheckInDateMustBeTodayOrYesterday = "O check-in vale para hoje ou para ontem";
    public const string CheckInAlreadyReplaced = "Esse check-in já foi substituído";
    public const string CheckInReplacementWindowHasClosed = "A troca só vale enquanto faltarem pelo menos 4 horas para a avaliação acabar";
    public const string OnlyCheckInOwnerCanReplace = "Só quem fez o check-in pode substituí-lo";
    public const string EnterReportReason = "Conta o motivo da denúncia";
    public const string ReportSuccessfullyRegistered = "Denúncia enviada. O dono e o admin já ficam sabendo";
    public const string ReportsSuccessfullyFound = "Denúncias encontradas";
    public const string OnlyOwnerOrAdminCanSeeReports = "Só o dono do desafio ou o admin da arena veem as denúncias";
    public const string UserHasReachedTheMaximumNumberOfCheckInsForTheDay = "Esse dia já está marcado";
    public const string EnterCheckInStatus = "Faltou o status do check-in";
    public const string EnterCheckInPhotoUrl = "Faltou a foto da prova";

    public const string DayOffSuccessfullyRegistered = "Folga usada. Descansa e volta mais forte";
    public const string EnterDayOffDate = "Escolhe o dia da folga";
    public const string EnterDayOffId = "Faltou o ID da folga";
    public const string EnterDayOffCreateDate = "Faltou a data de criação da folga";
    public const string DayOffAlreadyExistsForDate = "Esse dia já está de folga";
    public const string NoDayOffAvailable = "Suas folgas da semana acabaram";
    public const string OnlyRunningChallengesCanReceiveDayOffs = "Folga só vale com o desafio em andamento";
    public const string CheckInAlreadyExistsForDate = "Esse dia já tem check-in";
    public const string DayOffDateCannotBeInTheFuture = "Folga não vale para um dia que ainda não chegou";
    public const string DayOffDateOutsideChallengeRange = "Essa data está fora do período do desafio";
    public const string ChallengeRankingSuccessfullyFound = "Ranking do desafio na mesa";
    public const string EnterReviewerId = "Faltou quem avaliou";
    public const string InvalidReviewStatus = "Essa avaliação não vale";
    public const string InvalidReviewDate = "A data da avaliação não vale";
    public const string ProvideChallengeIdInOrderToUseAlsoOtherFilters = "Manda o ID do desafio para usar os outros filtros";
    #endregion

    #region Habit Messages
    public const string EnterHabitCreationDate = "Faltou a data de criação do hábito";
    public const string EnterHabitId = "Faltou o ID do hábito";
    public const string EnterHabitName = "Dá um nome para o hábito";
    public const string HabitCannotBeRemovedAsItHasLinkedChallenges = "Esse hábito ainda está em um desafio";
    public const string HabitNotFound = "Esse hábito sumiu do mapa";
    public const string HabitSuccessfullyDeleted = "Hábito removido";
    public const string HabitSuccessfullyFound = "Hábito encontrado";
    public const string HabitsSuccessfullyFound = "Hábitos encontrados";
    public const string HabitSuccessfullyRegistered = "Hábito criado. Bora transformar em desafio";
    public const string HabitSuccessfullyUpdated = "Hábito atualizado";
    public const string ThereIsAlreadyAHabitRegisteredWithThatName = "Já existe um hábito com esse nome";
    #endregion

    #region Challenge Messages
    public const string EnterOwnerId = "Faltou quem cria o desafio";
    public const string EnterChallengeEndDate = "Escolhe o dia em que o desafio termina";
    public const string EnterChallengeId = "Faltou o ID do desafio";
    public const string EnterChallengeCreationDate = "Faltou a data de criação do desafio";
    public const string EnterChallengeStatus = "Faltou o status do desafio";
    public const string EnterChallengeStartDate = "Escolhe o dia em que o desafio começa";
    public const string ChallengeHasNoMembers = "Esse desafio ainda está sem jogadores";
    public const string ChallengeNotFound = "Esse desafio sumiu do mapa";
    public const string EnterChallengeType = "Escolhe se o desafio é público ou privado";
    public const string EnterValidChallengeType = "Esse tipo de desafio não existe";
    public const string EnterValidDaysOffPerWeek = "As folgas por semana precisam ficar entre 0 e 6";
    public const string EnterValidPenaltyValue = "Escolhe um valor de multa que exista";
    public const string EnterPenaltyId = "Faltou o ID da multa";
    public const string PenaltyNotFound = "Essa multa sumiu do mapa";
    public const string PenaltyAlreadyPaid = "Essa multa já foi paga";
    public const string EnterExternalPaymentId = "Faltou o identificador do pagamento";
    public const string PixChargeSuccessfullyCreated = "PIX gerado. Paga e segue no jogo";
    public const string PixPaymentConfirmed = "PIX confirmado. Multa quitada!";
    public const string OnlyPenaltyOwnerCanCreatePixCharge = "Só quem tomou a multa pode gerar o PIX";
    public const string NotificationsSuccessfullyFound = "Avisos encontrados";
    public const string PenaltySuccessfullyPaid = "Multa paga. De volta para o jogo";
    public const string PenaltiesSuccessfullyFound = "Multas encontradas";
    public const string ChallengeStatusSuccessfullyUpdated = "Status do desafio atualizado";
    public const string ChallengeSuccessfullyCancelled = "Desafio cancelado. A arena fecha";
    public const string ChallengeSuccessfullyFound = "Desafio encontrado";
    public const string ChallengesSuccessfullyFound = "Desafios encontrados";
    public const string ChallengeSuccessfullyRegistered = "Desafio criado. Chama a galera!";
    #endregion

    #region Challenge Member Messages
    public const string EnterChallengeMemberId = "Faltou o ID do jogador no desafio";
    public const string EnterChallengeMemberJoinedDate = "Faltou a data de entrada no desafio";
    public const string OnlyNotStartedChallengesCanReceiveNewMembers = "Só dá para entrar antes do desafio começar";
    public const string OnlyNotStartedChallengesCanBeUpdated = "Só dá para mudar as datas antes do desafio começar";
    public const string ChallengeNeedsAtLeastTwoMembers = "O desafio precisa de pelo menos dois jogadores para começar";
    public const string MemberAlreadyAddedToTheChallenge = "Você já está nesse desafio";
    public const string MemberSuccessfullyAddedToTheChallenge = "Você entrou no desafio. Bora!";
    public const string MemberSuccessfullyReaddedToTheChallenge = "Você voltou para o desafio";
    public const string MemberSuccessfullyBlocked = "Jogador bloqueado";
    public const string MemberSuccessfullyUnblocked = "Jogador liberado";
    public const string MemberSuccessfullyRemovedFromTheChallenge = "Você saiu do desafio";
    public const string ChallengeMemberNotFound = "Esse jogador sumiu desse desafio";
    public const string ChallengeMemberStatusIdMustBeGreaterThanZero = "O status do jogador precisa ser maior que zero";
    public const string EnterChallengeMemberCreateDate = "Faltou a data de criação do jogador";
    public const string ChallengeMemberSuccessfullyFound = "Jogador encontrado";
    public const string MemberAlreadyBlockedInThisChallenge = "Esse jogador já está bloqueado";
    public const string MemberCannotBeRemovedFromTheChallengeAsHeIsBlocked = "Jogador bloqueado não sai do desafio por aqui";
    public const string MemberCanOnlyBeBlockedInRunningChallenges = "Só dá para bloquear com o desafio em andamento";
    public const string MemberCanOnlyBeRemovedFromNotStartedOrRunningChallenges = "Só dá para sair de um desafio que ainda não terminou";
    public const string MemberCanOnlyBeUnblockedInRunningChallenges = "Só dá para desbloquear com o desafio em andamento";
    public const string MemberHasAlreadyBeenOnThisChallengeBefore = "Esse jogador já passou por esse desafio";
    public const string MemberIsAlreadyBlockedInTheChallenge = "Esse jogador já está bloqueado";
    public const string MemberIsAlreadyActiveInTheChallenge = "Esse jogador já está na partida";
    public const string MemberIsBlockedInTheChallenge = "Você está bloqueado nesse desafio";
    public const string UserIsNotInTheChallenge = "Você não está nesse desafio";
    public const string UserHasNoChallenges = "Você ainda não entrou em nenhum desafio";
    #endregion

    #region User Messages
    public const string UserEmailAlreadyRegistered = "Esse e-mail já está no jogo";
    public const string EnterUserEmail = "Informa seu e-mail";
    public const string EnterUserId = "Faltou o ID do jogador";
    public const string EnterUserName = "Informa seu nome";
    public const string EnterUserNameOrEmail = "Informa seu nome ou e-mail";
    public const string EnterUserNickname = "Escolhe um apelido para a arena";
    public const string EnterUserPhoneNumber = "Informa seu celular";
    public const string EnterValidUserPhoneNumber = "Esse celular não vale";
    public const string EnterUserPixKey = "Informa sua chave PIX";
    public const string EnterValidUserPixKey = "Essa chave PIX não vale";
    public const string EnterValidCpfTaxIdentification = "Esse CPF não vale";
    public const string EnterValidCnpjTaxIdentification = "Esse CNPJ não vale";
    public const string EnterValidTaxIdentification = "Esse documento não vale";
    public const string EnterValidEVP = "Essa chave aleatória do PIX não vale";
    public const string EnterUserPixKeyType = "Escolhe o tipo da chave PIX";
    public const string EnterUserCreationDate = "Faltou a data de criação";
    public const string EnterValidUserEmail = "Esse e-mail não vale";
	public const string UserNotFound = "Esse jogador sumiu do mapa";
	public const string CheckInOwnerNotFound = "Quem fez o check-in sumiu do mapa";
	public const string UserSuccessfullyDeleted = "Conta removida";
    public const string UserSuccessfullyFound = "Jogador encontrado";
    public const string UsersSuccessfullyFound = "Jogadores encontrados";
    public const string UserSuccessfullyRegistered = "Conta criada. Bora para o primeiro desafio";
    public const string UserSuccessfullyUpdated = "Perfil atualizado";
    #endregion

    #region Review CheckIn Messages
    public const string CheckInReviewWindowHasAlreadyClosed = "A janela de avaliação já fechou";
    public const string ReviewAlreadyMadeByUserForThisCheckIn = "Você já deu o seu veredito nesse check-in";
    public const string ReviewerCannotReviewHisOwnCheckIn = "Você não julga o próprio check-in";
	#endregion

	#region Other Messages
	public const string ChangeNotMappedToCurrentStatus = "Essa mudança não vale nesse status";
    public const string EndDateMustBeLaterThanStartDate = "O fim precisa ser depois do começo";
    public const string OnlyValidGuidsCanBeCombined = "Só identificadores válidos podem ser combinados";
    public const string StartDateCannotBeEarlierThanToday = "O começo não pode ser antes de hoje";
    public const string StartDateMustBeFromTomorrow = "O começo precisa ser a partir de amanhã";
    public const string EnterPhoto = "Manda uma foto da prova";
    public const string EnterValidPhoto = "Essa foto não vale";
    #endregion

    #region Upload Messages
    public const string EnterUploadFileName = "Faltou o nome do arquivo";
    public const string EnterUploadContentType = "Faltou o tipo do arquivo";
    public const string AwsConfigNotConfigured = "O cofre de fotos não está configurado";
    public const string PresignedUploadUrlSuccessfullyGenerated = "Caminho da foto liberado";
    #endregion
}
