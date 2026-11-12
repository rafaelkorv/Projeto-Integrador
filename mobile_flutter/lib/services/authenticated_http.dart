import 'dart:convert';

import 'package:http/http.dart' as http_base;

import 'api_service.dart';
import 'auth_service.dart';

export 'package:http/http.dart'
    hide delete, get, patch, post, put, MultipartRequest;

final http_base.Client _client = _AuthenticatedClient(http_base.Client());

Map<String, String> _headersFor(Uri uri, Map<String, String>? headers) {
  final result = <String, String>{...?headers};
  final token = AuthService.authToken;
  if (token != null && uri.origin == Uri.parse(ApiService.baseUrl).origin) {
    result['Authorization'] = 'Bearer $token';
  }
  return result;
}

Future<http_base.Response> get(Uri url, {Map<String, String>? headers}) =>
    _client.get(url, headers: _headersFor(url, headers));

Future<http_base.Response> post(
  Uri url, {
  Map<String, String>? headers,
  Object? body,
  Encoding? encoding,
}) =>
    _client.post(
      url,
      headers: _headersFor(url, headers),
      body: body,
      encoding: encoding,
    );

Future<http_base.Response> put(
  Uri url, {
  Map<String, String>? headers,
  Object? body,
  Encoding? encoding,
}) =>
    _client.put(
      url,
      headers: _headersFor(url, headers),
      body: body,
      encoding: encoding,
    );

Future<http_base.Response> delete(
  Uri url, {
  Map<String, String>? headers,
  Object? body,
  Encoding? encoding,
}) =>
    _client.delete(
      url,
      headers: _headersFor(url, headers),
      body: body,
      encoding: encoding,
    );

Future<http_base.Response> patch(
  Uri url, {
  Map<String, String>? headers,
  Object? body,
  Encoding? encoding,
}) =>
    _client.patch(
      url,
      headers: _headersFor(url, headers),
      body: body,
      encoding: encoding,
    );

class MultipartRequest extends http_base.MultipartRequest {
  MultipartRequest(String method, Uri url) : super(method, url) {
    headers.addAll(_headersFor(url, null));
  }
}

class _AuthenticatedClient extends http_base.BaseClient {
  _AuthenticatedClient(this._inner);

  final http_base.Client _inner;

  @override
  Future<http_base.StreamedResponse> send(http_base.BaseRequest request) {
    request.headers.addAll(_headersFor(request.url, request.headers));
    return _inner.send(request);
  }
}
