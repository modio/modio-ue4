// Copyright 2019 modio. All Rights Reserved.
// Released under MIT.

#pragma once

#include "ModioHWrapper.h"
#include "FModioError.h"
#include "ModioResponse.generated.h"

USTRUCT(BlueprintType)
struct FModioResponse
{
  GENERATED_BODY()

  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  int32 Code;
  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  int32 ResultCount;
  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  int32 ResultLimit;
  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  int32 ResultOffset;
  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  int32 ResultTotal;
  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  bool ResultCached;
  UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "mod.io")
  FModioError Error;
};

extern void InitializeResponse( FModioResponse &Out_Response, const ModioResponse &ModioResponse);