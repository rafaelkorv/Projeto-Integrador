class Evento {
  final int id;
  final String titulo;
  final String? descricao;
  final String dataEvento; // yyyy-MM-dd
  final String horarioInicio; // HH:mm:ss
  final String horarioFim; // HH:mm:ss
  final String localEvento;
  final int? comunidadeId;
  final int? criadorId;
  final int? limiteParticipantes;
  final String status;
  final bool exigeCheckin;
  final String? encerramentoInscricoes;
  final String? situacaoTemporal;
  final bool? publico;
  final String? categoria;
  final String? imagemCapa;

  Evento({
    required this.id,
    required this.titulo,
    this.descricao,
    required this.dataEvento,
    required this.horarioInicio,
    required this.horarioFim,
    required this.localEvento,
    this.comunidadeId,
    this.criadorId,
    this.limiteParticipantes,
    this.status = 'AGENDADO',
    this.exigeCheckin = false,
    this.encerramentoInscricoes,
    this.situacaoTemporal,
    this.publico,
    this.categoria,
    this.imagemCapa,
  });

  static int _parseInt(dynamic val) {
    if (val == null) return 0;
    if (val is num) return val.toInt();
    if (val is String) return int.tryParse(val) ?? 0;
    if (val is Map && val.containsKey('id')) return _parseInt(val['id']);
    return 0;
  }

  static int? _parseNullableInt(dynamic val) {
    if (val == null) return null;
    if (val is num) return val.toInt();
    if (val is String) return int.tryParse(val);
    if (val is Map && val.containsKey('id')) return _parseNullableInt(val['id']);
    return null;
  }

  static String _parseDate(dynamic val) {
    if (val == null) return '';
    if (val is String) return val;
    if (val is List && val.isNotEmpty) {
      final year = val[0].toString().padLeft(4, '0');
      final month = val.length > 1 ? val[1].toString().padLeft(2, '0') : '01';
      final day = val.length > 2 ? val[2].toString().padLeft(2, '0') : '01';
      return '$year-$month-$day';
    }
    if (val is Map) {
      final year = (val['year'] ?? val['ano'] ?? 2026).toString().padLeft(4, '0');
      final month = (val['monthValue'] ?? val['month'] ?? val['mes'] ?? 1).toString().padLeft(2, '0');
      final day = (val['dayOfMonth'] ?? val['day'] ?? val['dia'] ?? 1).toString().padLeft(2, '0');
      return '$year-$month-$day';
    }
    return val.toString();
  }

  static String _parseTime(dynamic val) {
    if (val == null) return '';
    if (val is String) return val;
    if (val is List && val.isNotEmpty) {
      final hour = val[0].toString().padLeft(2, '0');
      final min = val.length > 1 ? val[1].toString().padLeft(2, '0') : '00';
      final sec = val.length > 2 ? val[2].toString().padLeft(2, '0') : '00';
      return '$hour:$min:$sec';
    }
    if (val is Map) {
      final hour = (val['hour'] ?? val['hora'] ?? 0).toString().padLeft(2, '0');
      final min = (val['minute'] ?? val['minuto'] ?? 0).toString().padLeft(2, '0');
      final sec = (val['second'] ?? val['segundo'] ?? 0).toString().padLeft(2, '0');
      return '$hour:$min:$sec';
    }
    return val.toString();
  }

  static String _parseString(dynamic val, [String fallback = '']) {
    if (val == null) return fallback;
    if (val is String) return val;
    return val.toString();
  }

  static bool _parseBool(dynamic val, [bool fallback = false]) {
    if (val == null) return fallback;
    if (val is bool) return val;
    if (val is num) return val != 0;
    if (val is String) return val.toLowerCase() == 'true' || val == '1';
    return fallback;
  }

  factory Evento.fromJson(Map<String, dynamic> json) {
    final parsedId = _parseInt(json['id'] ?? json['idEvento'] ?? json['id_evento']);

    return Evento(
      id: parsedId,
      titulo: _parseString(json['titulo'], 'Evento sem título'),
      descricao: json['descricao'] != null ? _parseString(json['descricao']) : null,
      dataEvento: _parseDate(json['dataEvento'] ?? json['data_evento'] ?? json['data']),
      horarioInicio: _parseTime(json['horarioInicio'] ?? json['horario_inicio'] ?? json['inicio']),
      horarioFim: _parseTime(json['horarioFim'] ?? json['horario_fim'] ?? json['fim']),
      localEvento: _parseString(json['localEvento'] ?? json['local_evento'] ?? json['local'], 'Local a confirmar'),
      comunidadeId: _parseNullableInt(json['comunidadeId'] ?? json['comunidade_id']),
      criadorId: _parseNullableInt(json['criadorId'] ?? json['criador_id']),
      limiteParticipantes: _parseNullableInt(json['limiteParticipantes'] ?? json['limite_participantes']),
      status: _parseString(json['status'], 'AGENDADO'),
      exigeCheckin: _parseBool(json['exigeCheckin'] ?? json['exige_checkin']),
      encerramentoInscricoes: json['encerramentoInscricoes'] != null ? _parseString(json['encerramentoInscricoes']) : null,
      situacaoTemporal: json['situacaoTemporal'] != null ? _parseString(json['situacaoTemporal']) : null,
      publico: json['publico'] != null ? _parseBool(json['publico']) : (json['comunidadeId'] == null),
      categoria: json['categoria'] != null ? _parseString(json['categoria']) : null,
      imagemCapa: json['imagemCapa'] != null ? _parseString(json['imagemCapa']) : (json['capa'] != null ? _parseString(json['capa']) : null),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'titulo': titulo,
      'descricao': descricao,
      'dataEvento': dataEvento,
      'horarioInicio': horarioInicio,
      'horarioFim': horarioFim,
      'localEvento': localEvento,
      if (comunidadeId != null) 'comunidadeId': comunidadeId,
      if (criadorId != null) 'criadorId': criadorId,
      if (limiteParticipantes != null) 'limiteParticipantes': limiteParticipantes,
      'status': status,
      'exigeCheckin': exigeCheckin,
      if (publico != null) 'publico': publico,
      if (categoria != null) 'categoria': categoria,
      if (imagemCapa != null) 'imagemCapa': imagemCapa,
    };
  }

  /// Retorna se o evento é estritamente público (não associado a comunidade privada e não cancelado)
  bool get ehPublico {
    if (status == 'CANCELADO') return false;
    if (publico != null) return publico!;
    return comunidadeId == null;
  }

  /// Retorna a situação real do evento baseada em data/hora local
  String get situacaoCalculada {
    final sit = situacaoTemporal ?? status;
    return sit;
  }

  /// Formata a data para exibição: "15 de ago. de 2026"
  String get dataFormatada {
    try {
      final parts = dataEvento.split('-');
      if (parts.length != 3) return dataEvento;
      final ano = parts[0];
      final mes = int.parse(parts[1]);
      final dia = parts[2];
      const meses = [
        '', 'jan', 'fev', 'mar', 'abr', 'mai', 'jun',
        'jul', 'ago', 'set', 'out', 'nov', 'dez'
      ];
      return '$dia de ${meses[mes]} de $ano';
    } catch (_) {
      return dataEvento;
    }
  }

  /// Retorna apenas HH:mm
  String get horarioFormatado {
    if (horarioInicio.length >= 5) {
      return horarioInicio.substring(0, 5);
    }
    return horarioInicio;
  }
}
