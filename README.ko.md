# 에이전트에서 Windows Fluent Icons를 찾으세요.

[![NuGet](https://img.shields.io/nuget/v/fluent-icons-mcp.svg)](https://www.nuget.org/packages/fluent-icons-mcp)
[![NuGet downloads](https://img.shields.io/nuget/dt/fluent-icons-mcp.svg)](https://www.nuget.org/packages/fluent-icons-mcp)
[![Pack and Publish](https://github.com/airtaxi/FluentIconsMcp/actions/workflows/pack-and-publish.yml/badge.svg)](https://github.com/airtaxi/FluentIconsMcp/actions/workflows/pack-and-publish.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)

🌐 [English](README.md) | 한국어

fluent-icons-mcp는 [WinUI Gallery](https://github.com/microsoft/WinUI-Gallery)의 Windows Fluent Icons 메타데이터를 stdio MCP 서버로 노출하는 .NET 글로벌 도구입니다. 로컬 코딩 에이전트가 이름, 태그, 코드로 아이콘 코드를 검색할 수 있습니다.

## 주요 기능

- Codex, Claude Code, GitHub Copilot CLI, Gemini CLI 또는 stdio MCP 서버를 지원하는 임의의 provider에서 Windows Fluent Icons 검색 도구를 사용할 수 있습니다.
- `mcp-install`, `mcp-remove`, `mcp-uninstall` 명령으로 지원 provider에 MCP 서버를 등록하거나 제거합니다.
- provider 설치 명령이 없는 환경을 위해 임의의 `mcp.json` 파일을 직접 편집할 수 있습니다.
- 온라인 상태에서는 최신 WinUI Gallery `IconsData.json`을 다운로드하고, 오프라인 사용을 위해 캐시합니다.
- 아이콘의 code, name, tags, `isSegoeFluentOnly` 정보를 반환합니다.

## 설치

```powershell
dotnet tool install --global fluent-icons-mcp
```

기존 설치를 업데이트하려면 다음을 실행합니다.

```powershell
dotnet tool update --global fluent-icons-mcp
```

## Provider 설정

지원되는 provider CLI를 통해 등록합니다.

```powershell
fluent-icons-mcp mcp-install --provider codex
fluent-icons-mcp mcp-install --provider claude
fluent-icons-mcp mcp-install --provider copilot
fluent-icons-mcp mcp-install --provider gemini
fluent-icons-mcp mcp-install --provider all
```

등록을 제거하려면 다음을 실행합니다.

```powershell
fluent-icons-mcp mcp-remove --provider codex
fluent-icons-mcp mcp-uninstall --provider claude
fluent-icons-mcp mcp-remove --provider all
```

기본 MCP 서버 이름은 `fluent-icons`입니다. 필요하면 이름을 지정할 수 있습니다.

```powershell
fluent-icons-mcp mcp-install --provider codex --name fluent-icons
```

## 사용자 지정 mcp.json

다른 provider에서는 대상 MCP 설정 파일을 직접 지정합니다.

```powershell
fluent-icons-mcp mcp-install --config "C:\path\to\mcp.json"
```

다음 항목이 추가됩니다.

```json
{
  "mcpServers": {
    "fluent-icons": {
      "command": "fluent-icons-mcp",
      "args": []
    }
  }
}
```

설정 편집기는 읽을 때 JSON 주석과 trailing comma를 허용합니다. 저장할 때는 표준 JSON으로 정규화하며, 기존 설정 파일을 수정하기 전에 `.bak` 백업을 만듭니다.

## MCP 도구

- `search_fluent_icons`: Windows Fluent Icons를 `name`, `tags`, `code`로 검색하고 일치하는 아이콘 메타데이터를 반환합니다.

도구는 선택적으로 `name`, `tags`, `code`, `maxResults` 파라미터를 받습니다. 이름과 태그 검색은 대소문자를 구분하지 않는 부분 일치입니다. 여러 태그 구문은 쉼표 또는 세미콜론으로 구분할 수 있으며, 모든 구문이 하나 이상의 아이콘 태그와 일치해야 합니다.

아이콘 데이터는 서버 시작 시 WinUI Gallery에서 다운로드하고 `%LOCALAPPDATA%\FluentIconsMcp\IconsData.json`에 캐시합니다. 다운로드에 실패하면 캐시 파일을 사용합니다.

## 수동 서버 명령

인자 없이 실행하면 stdio MCP 서버가 시작됩니다.

```powershell
fluent-icons-mcp
```

CLI 도움말은 다음으로 확인합니다.

```powershell
fluent-icons-mcp help
fluent-icons-mcp --help
```

## 패키지 개발

```powershell
dotnet restore
dotnet build
dotnet test
dotnet pack
```

## 라이선스

fluent-icons-mcp는 [MIT License](LICENSE.txt)로 배포됩니다.

Fluent Icons 메타데이터는 [microsoft/WinUI-Gallery](https://github.com/microsoft/WinUI-Gallery)에서 가져옵니다. 자세한 고지는 [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md)를 확인해 주세요.

## 작성자

[Howon Lee (airtaxi)](https://github.com/airtaxi)가 만들었습니다.

OpenAI Codex의 도움을 받아 제작했습니다.
